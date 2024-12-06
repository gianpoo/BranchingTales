using StoryTeller.UseCases.Chats.GetPrompts;
using StoryTeller.Web.Prompts;

namespace StoryTeller.Web.Chats;

public class GetPrompts : Endpoint<GetChatPromptsRequest, PromptListResponse>
{
    private readonly IMediator _mediator;

    public GetPrompts(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/Chats/{ChatId}/Prompts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        GetChatPromptsRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetChatPromptsQuery(request.ChatId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        if (result.IsSuccess)
        {
            Response = new PromptListResponse(result.Value);
            return;
        }

        await SendNotFoundAsync(cancellationToken);
    }
} 