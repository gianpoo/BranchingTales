using StoryTeller.UseCases.Prompts.Create;
using StoryTeller.Core.Interfaces;

namespace StoryTeller.Web.Prompts;

public class Create : Endpoint<CreatePromptRequest, CreatePromptResponse>
{
    private readonly IMediator _mediator;

    public Create(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/Chats/prompts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        CreatePromptRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Text))
        {
            ThrowError("Prompt text is required");
        }

        var command = new CreatePromptCommand(request.Text!);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            Response = new CreatePromptResponse(1, request.Text!, result.Value);
            return;
        }

        AddError(result.Errors.FirstOrDefault() ?? "Failed to create prompt");
    }
} 