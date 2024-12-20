using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using StoryTeller.Core.Interfaces;
using StoryTeller.Core.Settings;

namespace StoryTeller.Infrastructure.Services;

public class AzureOpenAIService : IAIService
{
    private readonly ILogger<AzureOpenAIService> _logger;
    private Kernel? _kernel;
    private readonly AzureOpenAISettings _settings;

    public static async Task<AzureOpenAIService> CreateAsync(
        IOptions<AzureOpenAISettings> settings,
        ILogger<AzureOpenAIService> logger)
    {
        var service = new AzureOpenAIService(settings, logger);
        await service.InitializeAsync();
        return service;
    }

    private AzureOpenAIService(
        IOptions<AzureOpenAISettings> settings,
        ILogger<AzureOpenAIService> logger)
    {
        _logger = logger;
        _settings = settings.Value;
    }

    private async Task InitializeAsync()
    {
        try
        {
            var builder = Kernel.CreateBuilder()
                .AddAzureOpenAIChatCompletion(
                    deploymentName: _settings.DeploymentName,
                    endpoint: _settings.Endpoint,
                    apiKey: _settings.ApiKey);

            _kernel = builder.Build();
            
            // Test the connection with a proper function definition
            var promptConfig = new PromptTemplateConfig
            {
                Template = "Hello",
                ExecutionSettings = new Dictionary<string, PromptExecutionSettings>
                {
                    {
                        "default", new OpenAIPromptExecutionSettings
                        {
                            Temperature = _settings.Temperature,
                            MaxTokens = _settings.MaxTokens
                        }
                    }
                }
            };

            var testFunction = Kernel.CreateFunctionFromPrompt(promptConfig);
            var testResult = await _kernel.InvokeAsync(testFunction, new KernelArguments());

            if (testResult.GetValue<string>() is not { Length: > 0 })
            {
                throw new InvalidOperationException("Failed to get valid response during initialization");
            }

            _logger.LogInformation("Azure OpenAI service initialized successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize Azure OpenAI service");
            throw;
        }
    }

    private Kernel Kernel => _kernel ?? 
        throw new InvalidOperationException("Service not properly initialized");

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
                    Make them engaging and descriptive, but keep them concise. Format each option as a numbered list on newlines starting with "#) ".
                    """,
                ExecutionSettings = new Dictionary<string, PromptExecutionSettings>
                {
                    {
                        "default", new OpenAIPromptExecutionSettings
                        {
                            Temperature = _settings.Temperature,
                            MaxTokens = _settings.MaxTokens
                        }
                    }
                }
            };

            var function = Kernel.CreateFunctionFromPrompt(promptConfig);
            var result = await Kernel.InvokeAsync(function, arguments, cancellationToken);

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
        int currentIteration = 1,
        int totalIterations = 3,
        int numberOfOptions = 3)
    {
        try
        {
            var promptConfig = new PromptTemplateConfig
            {
                Template = $"""
                    Based on: {context}
                    Part {currentIteration}/{totalIterations}
                    Generate {numberOfOptions} brief story continuations.
                    Format: number-dash-text
                    Example:
                    1-text
                    2-text
                    Keep each under 50 words.
                    """,
                ExecutionSettings = new Dictionary<string, PromptExecutionSettings>
                {
                    {
                        "default", new OpenAIPromptExecutionSettings
                        {
                            Temperature = _settings.Temperature,
                            MaxTokens = _settings.MaxTokens
                        }
                    }
                }
            };

            var function = Kernel.CreateFunctionFromPrompt(promptConfig);
            var result = await Kernel.InvokeAsync(function, new KernelArguments());

            if (result.GetValue<string>() is not { Length: > 0 } content)
            {
                _logger.LogWarning("Empty response from AI service");
                return GetFallbackOptions();
            }

            var options = content
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Where(line => line.Trim().Length > 0 && char.IsDigit(line.Trim()[0]))
                .Select(line => 
                {
                    var dashIndex = line.IndexOf('-');
                    return dashIndex >= 0 ? line[(dashIndex + 1)..].Trim() : line.Trim();
                })
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Take(numberOfOptions)
                .ToList();

            if (!options.Any())
            {
                _logger.LogWarning("No valid options parsed from AI response");
                return GetFallbackOptions();
            }

            return options;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating story options for context: {Context}", context);
            return GetFallbackOptions();
        }
    }

    private List<string> GetFallbackOptions() => new()
    {
        "The story continues along a safe path.",
        "The characters take a moment to assess their situation.",
        "An unexpected but manageable event occurs."
    };
}
