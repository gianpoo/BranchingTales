using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using StoryTeller.Core.Interfaces;
using StoryTeller.Core.Settings;

namespace StoryTeller.Infrastructure.Services;

public class AzureOpenAIService : IAIService
{
    private readonly ILogger<AzureOpenAIService> _logger;
    private readonly Kernel _kernel;
    private readonly AzureOpenAISettings _settings;

    public AzureOpenAIService(
        IOptions<AzureOpenAISettings> settings,
        ILogger<AzureOpenAIService> logger)
    {
        _logger = logger;
        _settings = settings.Value;

        // Create kernel builder with default settings
        var builder = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(
                deploymentName: _settings.DeploymentName,
                endpoint: _settings.Endpoint,
                apiKey: _settings.ApiKey);

        // Build the kernel
        _kernel = builder.Build();
    }

    public async Task<string> GenerateStoryResponseAsync(
        string prompt,
        int numberOfOptions = 3,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var arguments = new KernelArguments
            {
                { "input", prompt },
                { "numberOfOptions", numberOfOptions }
            };

            var promptConfig = new PromptTemplateConfig
            {
                Template = """
                    You are a creative storyteller. Create a short story prompt based on the following context:
                    {{$input}}
                    
                    Generate {{$numberOfOptions}} possible continuations to the story in a way that maintains consistency with the previous events.
                    Make them engaging and descriptive, but keep them concise. Format each option as a numbered list on newlines stawting with "#) ".
                    """,
                ExecutionSettings = new Dictionary<string, PromptExecutionSettings>
                {
                    {
                        "default", new OpenAIPromptExecutionSettings
                        {
                            Temperature = _settings.Temperature,
                            MaxTokens = _settings.MaxTokens,
                            TopP = _settings.TopP,
                            FrequencyPenalty = _settings.FrequencyPenalty,
                            PresencePenalty = _settings.PresencePenalty
                        }
                    }
                }
            };

            var function = _kernel.CreateFunctionFromPrompt(promptConfig);
            var result = await _kernel.InvokeAsync(function, arguments, cancellationToken);

            if (result.GetValue<string>() is not { Length: > 0 } content)
            {
                throw new InvalidOperationException("Failed to generate story response");
            }

            return content.Trim();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating story response for prompt: {Prompt}", prompt);
            throw;
        }
    }

    public async Task<List<string>> GenerateStoryOptionsAsync(
        string context,
        int numberOfOptions = 3,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var arguments = new KernelArguments
            {
                { "context", context },
                { "numberOfOptions", numberOfOptions }
            };

            var promptConfig = new PromptTemplateConfig
            {
                Template = """
                    Story so far:
                    {{$context}}

                    Generate {{$numberOfOptions}} different possible continuations that are consistent with the previous events.
                    Each option should be unique, interesting, and offer a clear direction for the story.
                    Format each option on a new line starting with "#) ".
                    Keep each option concise but engaging.
                    """,
                ExecutionSettings = new Dictionary<string, PromptExecutionSettings>
                {
                    {
                        "default", new OpenAIPromptExecutionSettings
                        {
                            Temperature = _settings.Temperature,
                            MaxTokens = _settings.MaxTokens,
                            TopP = _settings.TopP,
                            FrequencyPenalty = _settings.FrequencyPenalty,
                            PresencePenalty = _settings.PresencePenalty
                        }
                    }
                }
            };

            var function = _kernel.CreateFunctionFromPrompt(promptConfig);
            var result = await _kernel.InvokeAsync(function, arguments, cancellationToken);

            if (result.GetValue<string>() is not { Length: > 0 } content)
            {
                throw new InvalidOperationException("Failed to generate story options");
            }

            var storyOptions = content
                .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(line => line.StartsWith("-"))
                .Select(line => line.TrimStart('-', ' '))
                .Take(numberOfOptions)
                .ToList();

            return storyOptions.Any() ? storyOptions : new List<string> { "The story continues..." };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating story options for context: {Context}", context);
            throw;
        }
    }
}
