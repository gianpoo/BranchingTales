using StoryTeller.Core.ContributorAggregate;
using StoryTeller.Core.ContributorAggregate.Specifications;

namespace StoryTeller.UseCases.Contributors.SearchByName;

public class SearchByNameHandler(IReadRepository<Contributor> _repository)
    : IQueryHandler<SearchByNameQuery, Result<List<ContributorDTO>>>
{
    public async Task<Result<List<ContributorDTO>>> Handle(SearchByNameQuery request, 
        CancellationToken cancellationToken)
    {
        var spec = new ContributorByNameSpec(request.Name);
        var entities = await _repository.ListAsync(spec, cancellationToken);
        
        var dtos = entities.Select(e => 
            new ContributorDTO(e.Id, e.Name, e.PhoneNumber?.Number ?? "")).ToList();
            
        return dtos;
    }
} 