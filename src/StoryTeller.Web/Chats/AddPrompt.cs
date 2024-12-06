using StoryTeller.UseCases.Prompts.Create;
using StoryTeller.Web.Prompts;

namespace StoryTeller.Web.Chats;

public class AddPrompt : Endpoint<AddPromptRequest, PromptRecord>
{
    private readonly IMediator _mediator;

    public AddPrompt(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/Chats/{ChatId}/Prompts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        AddPromptRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Text))
        {
            ThrowError("Prompt text is required");
        }

        var command = new CreatePromptCommand(request.Text);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            Response = new PromptRecord(1, request.Text);
            return;
        }

        await SendNotFoundAsync(cancellationToken);
    }
}

public class AddPromptRequest
{
    public int ChatId { get; set; }
    public string Text { get; set; } = string.Empty;
} 