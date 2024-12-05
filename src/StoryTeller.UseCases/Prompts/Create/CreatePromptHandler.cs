using System.Linq;
using StoryTeller.Core.ChatAggregate;
using StoryTeller.Core.DTOs;
using StoryTeller.Core.Interfaces;

namespace StoryTeller.UseCases.Prompts.Create;

public class CreatePromptHandler : ICommandHandler<CreatePromptCommand, Result<string>>
{
    private readonly IChatRepository _repository;

    public CreatePromptHandler(IChatRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<string>> Handle(CreatePromptCommand request, CancellationToken cancellationToken)
    {
        var chat = await _repository.GetByIdAsync(1);
        if (chat == null)
        {
            return Result<string>.NotFound();
        }

        await _repository.AddPromptAsync(request.Text);
        var promptId = chat.Prompts.Count + 1;
        var path = $"https://localhost:57679/Chats/1/Prompts/{promptId}";
        return Result<string>.Success(path);
    }
} 