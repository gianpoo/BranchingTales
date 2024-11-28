using StoryTeller.UseCases.Prompts.Create;

namespace StoryTeller.Web.Prompts;

public class Create(IMediator _mediator)
    : Endpoint<CreatePromptRequest, CreatePromptResponse>
{
    public override void Configure()
    {
        Post("/Prompts");
        AllowAnonymous();
        Summary(s =>
        {
            s.ExampleRequest = new CreatePromptRequest { Text = "Example prompt text" };
        });
    }

    public override async Task HandleAsync(
        CreatePromptRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreatePromptCommand(request.Text!), 
            cancellationToken);

        if (result.IsSuccess)
        {
            Response = new CreatePromptResponse(result.Value, request.Text!);
            return;
        }
        // TODO: Handle other cases as necessary
    }
} 