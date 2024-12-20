using StoryTeller.Core.Interfaces;

namespace StoryTeller.Web.Chats;

public class GetOptions : Endpoint<GetOptionsRequest, OptionsResponse>
{
    private readonly IResponseService _responseService;

    public GetOptions(IResponseService responseService)
    {
        _responseService = responseService;
    }

    public override void Configure()
    {
        Get("/Chats/{ChatId}/options");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        GetOptionsRequest request,
        CancellationToken cancellationToken)
    {
        try 
        {
            var options = await _responseService.GetOptionsAsync();
            Response = new OptionsResponse(options);
        }
        catch (Exception ex)
        {
            await SendErrorsAsync(cancellation: cancellationToken);
            Logger.LogError(ex, "Error getting options");
        }
    }
}

public class GetOptionsRequest
{
    public int ChatId { get; set; }
}

public record OptionsResponse(List<string> Options); 