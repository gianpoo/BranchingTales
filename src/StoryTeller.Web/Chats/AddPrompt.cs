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
        try
        {
            if (string.IsNullOrEmpty(request.Text))
            {
                ThrowError("Text is required");
            }

            var command = new CreatePromptCommand(request.Text);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                Logger.LogError("Failed to add prompt: {Error}", result.Errors.FirstOrDefault());
                ThrowError(result.Errors.FirstOrDefault() ?? "Failed to add prompt");
                return;
            }

            Response = new PromptRecord(request.ChatId, request.Text);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error adding prompt");
            ThrowError("An error occurred while processing your request");
        }
    }
}

public class AddPromptRequest
{
    public int ChatId { get; set; }
    public string Text { get; set; } = string.Empty;
} 