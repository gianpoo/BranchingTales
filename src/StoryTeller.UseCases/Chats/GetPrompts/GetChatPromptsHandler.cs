using StoryTeller.Core.DTOs;
using StoryTeller.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace StoryTeller.UseCases.Chats.GetPrompts;

public class GetChatPromptsHandler : IQueryHandler<GetChatPromptsQuery, Result<List<PromptDTO>>>
{
    private readonly IChatRepository _repository;
    private readonly ILogger<GetChatPromptsHandler> _logger;

    public GetChatPromptsHandler(IChatRepository repository, ILogger<GetChatPromptsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<List<PromptDTO>>> Handle(GetChatPromptsQuery request, CancellationToken cancellationToken)
    {
        var chat = await _repository.GetByIdAsync(request.ChatId);
        
        if (chat == null)
        {
            _logger.LogWarning("Chat not found for ID: {ChatId}", request.ChatId);
            return Result<List<PromptDTO>>.NotFound();
        }

        _logger.LogInformation("Found chat with {PromptCount} prompts", chat.Prompts.Count);
        
        var prompts = chat.Prompts.Select(p => new PromptDTO(p.Id, p.Text)).ToList();
        return Result.Success(prompts);
    }
} 