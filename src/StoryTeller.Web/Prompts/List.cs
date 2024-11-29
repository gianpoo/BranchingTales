using StoryTeller.UseCases.Prompts;
using StoryTeller.UseCases.Prompts.List;

namespace StoryTeller.Web.Prompts;

/// <summary>
/// List all Contributors
/// </summary>
/// <remarks>
/// List all contributors - returns a ContributorListResponse containing the Contributors.
/// </remarks>
public class List(IMediator _mediator) : EndpointWithoutRequest<PromptListResponse>
{
  public override void Configure()
  {
    Get("/Prompts");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken cancellationToken)
  {
    Result<IEnumerable<PromptDTO>> result = await _mediator.Send(new ListPromptsQuery(null, null), cancellationToken);

    if (result.IsSuccess)
    {
      Response = new PromptListResponse
      {
        Prompts = result.Value.Select(p => new PromptRecord(p.Id, p.Text)).ToList()
      };
    }
  }
}
