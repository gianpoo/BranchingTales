using StoryTeller.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace StoryTeller.UseCases.Prompts.Create;

public class CreatePromptHandler : ICommandHandler<CreatePromptCommand, Result<string>>
{
    private readonly IChatRepository _repository;
    private readonly IAIService _aiService;
    private readonly IResponseService _responseService;
    private readonly ILogger<CreatePromptHandler> _logger;

    public CreatePromptHandler(
        IChatRepository repository,
        IAIService aiService,
        IResponseService responseService,
        ILogger<CreatePromptHandler> logger)
    {
        _repository = repository;
        _aiService = aiService;
        _responseService = responseService;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(CreatePromptCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Add prompt to chat
            await _repository.AddPromptAsync(request.Text);
            
            // Get updated chat for context
            var chat = await _repository.GetChat();
            if (chat == null)
            {
                return Result<string>.Error("Chat not found");
            }

            // Generate new options based on full context
            var context = string.Join("\n", chat.Prompts.Select(p => p.Text));
            var currentIteration = chat.Prompts.Count;
            
            var options = await _aiService.GenerateStoryOptionsAsync(
                context,
                currentIteration: currentIteration,
                totalIterations: chat.Limit);
            
            // Save new options
            await _responseService.SaveOptionsAsync(options);

            return Result<string>.Success($"/Chats/1/Prompts/{chat.Prompts.Count}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding prompt");
            return Result<string>.Error("Failed to save prompt");
        }
    }
} 
