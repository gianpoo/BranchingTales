namespace StoryTeller.UseCases.Contributors.SearchByName;

public record SearchByNameQuery(string Name) : IQuery<Result<List<ContributorDTO>>>; 