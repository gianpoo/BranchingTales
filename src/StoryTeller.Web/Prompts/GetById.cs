using StoryTeller.UseCases.Prompts.Get;

namespace StoryTeller.Web.Prompts;

public class GetById(IMediator _mediator)
    : Endpoint<GetPromptByIdRequest, PromptRecord>
{
    public override void Configure()
    {
        Get("/Prompts/{PromptId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        GetPromptByIdRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetPromptQuery(request.PromptId);

        var result = await _mediator.Send(query, cancellationToken);

        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        if (result.IsSuccess)
        {
            Response = new PromptRecord(result.Value.Id, result.Value.Text);
        }
    }
} 