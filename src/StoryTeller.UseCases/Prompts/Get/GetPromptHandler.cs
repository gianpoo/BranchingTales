using System.Linq;
using StoryTeller.Core.ChatAggregate;
using StoryTeller.Core.DTOs;
using StoryTeller.Core.Interfaces;

namespace StoryTeller.UseCases.Prompts.Get;

public class GetPromptHandler : IQueryHandler<GetPromptQuery, Result<PromptDTO>>
{
    private readonly IChatRepository _repository;

    public GetPromptHandler(IChatRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PromptDTO>> Handle(GetPromptQuery request, CancellationToken cancellationToken)
    {
        var chat = await _repository.GetByIdAsync(1);
        if (chat == null)
        {
            return Result<PromptDTO>.NotFound();
        }

        var prompt = chat.Prompts.FirstOrDefault(p => p.Id == request.PromptId);
        if (prompt == null)
        {
            return Result<PromptDTO>.NotFound();
        }

        return Result.Success(new PromptDTO(prompt.Id, prompt.Text));
    }
}