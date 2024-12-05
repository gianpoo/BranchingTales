using StoryTeller.UseCases.Prompts.Get;
using StoryTeller.Core.Interfaces;

namespace StoryTeller.Web.Prompts;

public class GetById : Endpoint<GetPromptByIdRequest, PromptRecord>
{
    private readonly IMediator _mediator;

    public GetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/Chats/prompts/{PromptId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        GetPromptByIdRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetPromptQuery(request.PromptId);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        Response = new PromptRecord(result.Value.Id, result.Value.Text);
    }
} 