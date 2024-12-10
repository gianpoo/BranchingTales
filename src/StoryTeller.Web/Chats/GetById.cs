using StoryTeller.Core.Interfaces;
using StoryTeller.UseCases.Chats.Get;
using StoryTeller.Web.Prompts;

namespace StoryTeller.Web.Chats;

public class GetById : Endpoint<GetChatByIdRequest, ChatResponse>
{
    private readonly IMediator _mediator;

    public GetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/Chats/{ChatId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        GetChatByIdRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetChatQuery(request.ChatId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        if (result.IsSuccess)
        {
            Response = new ChatResponse(
                result.Value.Prompts.Select(p => new PromptRecord(p.Id, p.Text)).ToList(),
                result.Value.Limit);
        }
    }
} 