using System.Linq;
using StoryTeller.Core.ChatAggregate;
using StoryTeller.Core.DTOs;
using StoryTeller.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace StoryTeller.UseCases.Prompts.Create;

public class CreatePromptHandler : ICommandHandler<CreatePromptCommand, Result<string>>
{
    private readonly IChatRepository _repository;
    private readonly ILogger<CreatePromptHandler> _logger;

    public CreatePromptHandler(IChatRepository repository, ILogger<CreatePromptHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(CreatePromptCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.AddPromptAsync(request.Text);
            var chat = await _repository.GetByIdAsync(1);
            if (chat == null)
            {
                _logger.LogError("Failed to retrieve chat after adding prompt");
                return Result<string>.Error("Failed to save prompt");
            }

            var promptId = chat.Prompts.Count;
            var path = $"https://localhost:57679/Chats/1/Prompts/{promptId}";
            return Result<string>.Success(path);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding prompt: {Text}", request.Text);
            return Result<string>.Error("Failed to save prompt");
        }
    }
} 