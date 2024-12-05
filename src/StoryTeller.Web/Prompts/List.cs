using StoryTeller.Core.DTOs;
using StoryTeller.Core.Interfaces;
using StoryTeller.UseCases.Prompts.List;

namespace StoryTeller.Web.Prompts;

public class List : EndpointWithoutRequest<PromptListResponse>
{
    private readonly IMediator _mediator;

    public List(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/Chats/prompts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var query = new ListPromptsQuery();
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            Response = new PromptListResponse(result.Value);
            return;
        }

        AddError(result.Errors.FirstOrDefault() ?? "Failed to list prompts");
    }
}
