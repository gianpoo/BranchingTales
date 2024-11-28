namespace StoryTeller.Web.Contributors;
using StoryTeller.UseCases.Contributors.SearchByName;

public class Search(IMediator _mediator) 
    : Endpoint<SearchContributorRequest, List<ContributorRecord>>
{
    public override void Configure()
    {
        Get(SearchContributorRequest.Route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        SearchContributorRequest request,
        CancellationToken cancellationToken)
    {
        var query = new SearchByNameQuery(request.SearchName);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            Response = result.Value
                .Select(c => new ContributorRecord(c.Id, c.Name, c.PhoneNumber))
                .ToList();
        }
    }
} 