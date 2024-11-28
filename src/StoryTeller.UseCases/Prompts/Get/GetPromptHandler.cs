using StoryTeller.Core.PromptAggregate;
using StoryTeller.Core.PromptAggregate.Specifications;

namespace StoryTeller.UseCases.Prompts.Get;

public class GetPromptHandler : IQueryHandler<GetPromptQuery, Result<PromptDTO>>
{
    private readonly IReadRepository<Prompt> _repository;

    public GetPromptHandler(IReadRepository<Prompt> repository)
    {
        _repository = repository;
    }

    public async Task<Result<PromptDTO>> Handle(GetPromptQuery request, CancellationToken cancellationToken)
    {
        var spec = new PromptByIdSpec(request.PromptId);
        var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
        if (entity == null) return Result.NotFound();

        return new PromptDTO(entity.Id, entity.Text);
    }
} 