using System.Text.Json;
using StoryTeller.Core.Interfaces;

namespace StoryTeller.Web.Chats;

public class GetRandomResponse : EndpointWithoutRequest<RandomResponseResult>
{
    private readonly IResponseService _responseService;

    public GetRandomResponse(IResponseService responseService)
    {
        _responseService = responseService;
    }

    public override void Configure()
    {
        Get("/Chats/responses/random");
        AllowAnonymous();
    }

    public override Task HandleAsync(CancellationToken cancellationToken)
    {
        var response = _responseService.GetRandomResponse();
        Response = new RandomResponseResult(JsonSerializer.Deserialize<List<string>>(response)!);
        return Task.CompletedTask;
    }
}

public record RandomResponseResult(List<string> Options); 