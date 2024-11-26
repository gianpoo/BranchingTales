using StoryTeller.Core.ContributorAggregate;

namespace StoryTeller.UseCases.Contributors.Create;

public class CreateContributorHandler : ICommandHandler<CreateContributorCommand, Result<int>>
{
  private readonly IRepository<Contributor> _repository;

  public CreateContributorHandler(IRepository<Contributor> repository)
  {
    _repository = repository;
  }

  public async Task<Result<int>> Handle(CreateContributorCommand request, CancellationToken cancellationToken)
  {
    // Validate input if necessary
    if (string.IsNullOrEmpty(request.Name))
    {
      return Result<int>.Error("Name is required.");
    }

    // Create a new Contributor instance
    var newContributor = new Contributor(request.Name);

    // Set the phone number if provided
    if (!string.IsNullOrEmpty(request.PhoneNumber))
    {
      newContributor.SetPhoneNumber(request.PhoneNumber);
    }

    // Add the contributor to the repository
    var createdItem = await _repository.AddAsync(newContributor, cancellationToken);

    // Return success with the created contributor's ID
    return Result<int>.Success(createdItem.Id);
  }
}
