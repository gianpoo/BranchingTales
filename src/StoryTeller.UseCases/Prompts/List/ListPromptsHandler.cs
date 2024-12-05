using Ardalis.Result;
using StoryTeller.Core.DTOs;
using StoryTeller.Core.Interfaces;
using MediatR;

namespace StoryTeller.UseCases.Prompts.List;

public class ListPromptsHandler : IRequestHandler<ListPromptsQuery, Result<IEnumerable<PromptDTO>>>
{
    private readonly IChatRepository _repository;

    public ListPromptsHandler(IChatRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<PromptDTO>>> Handle(
        ListPromptsQuery request,
        CancellationToken cancellationToken)
    {
        var chat = await _repository.GetChat();
        if (chat == null)
        {
            return Result.NotFound();
        }

        var prompts = chat.Prompts.Select(p => new PromptDTO(p.Id, p.Text));
        return Result.Success(prompts);
    }
}
