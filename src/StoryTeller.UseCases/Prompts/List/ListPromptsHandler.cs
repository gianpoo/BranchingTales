namespace StoryTeller.UseCases.Prompts.List;
public class ListPromptsHandler(IListPromptsQueryService _query)
  : IQueryHandler<ListPromptsQuery, Result<IEnumerable<PromptDTO>>>
{
  public async Task<Result<IEnumerable<PromptDTO>>> Handle(ListPromptsQuery request, CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync();

    return Result.Success(result);
  }
}
