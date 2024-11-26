namespace StoryTeller.UseCases.Contributors.Update;

public record UpdateContributorCommand(int ContributorId, string NewName, string? NewPhoneNumber) : ICommand<Result<ContributorDTO>>;
