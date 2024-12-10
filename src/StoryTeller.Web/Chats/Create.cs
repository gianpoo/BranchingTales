using StoryTeller.Core.Interfaces;
using StoryTeller.UseCases.Chats.Create;
using StoryTeller.Web.Prompts;

namespace StoryTeller.Web.Chats;

public class Create : Endpoint<CreateChatRequest, ChatResponse>
{
    private readonly IMediator _mediator;

    public Create(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/Chats");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        CreateChatRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Text))
        {
            ThrowError("Message text is required");
        }

        if (request.Limit <= 0)
        {
            ThrowError("Limit must be greater than zero");
        }

        var command = new CreateChatCommand(request.Text!, request.Limit);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            Response = new ChatResponse(
                new List<PromptRecord> { new(1, request.Text!) },
                request.Limit
            );
            return;
        }

        AddError(result.Errors.FirstOrDefault() ?? "Failed to create chat");
    }
} 