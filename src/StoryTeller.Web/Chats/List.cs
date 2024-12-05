using StoryTeller.Core.DTOs;
using StoryTeller.Core.Interfaces;
using StoryTeller.UseCases.Chats.List;
using StoryTeller.Web.Prompts;

namespace StoryTeller.Web.Chats;

public class List : EndpointWithoutRequest<List<ChatResponse>>
{
    private readonly IMediator _mediator;

    public List(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/Chats");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var query = new ListChatsQuery();
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            Response = result.Value.Select(c => new ChatResponse(
                c.Prompts.Select(p => new PromptRecord(p.Id, p.Text)).ToList())).ToList();
        }
    }
} 