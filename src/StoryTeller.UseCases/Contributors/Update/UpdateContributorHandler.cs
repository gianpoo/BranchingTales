using StoryTeller.Core.ContributorAggregate;

namespace StoryTeller.UseCases.Contributors.Update;

public class UpdateContributorHandler(IRepository<Contributor> _repository)
  : ICommandHandler<UpdateContributorCommand, Result<ContributorDTO>>
{
  public async Task<Result<ContributorDTO>> Handle(UpdateContributorCommand request, CancellationToken cancellationToken)
  {
    var existingContributor = await _repository.GetByIdAsync(request.ContributorId, cancellationToken);
    if (existingContributor == null)
    {
      return Result.NotFound();
    }

    // Update Name
    existingContributor.UpdateName(request.NewName!);

    // Update Phone Number if provided (it's optional)
    if (!string.IsNullOrEmpty(request.NewPhoneNumber))
    {
      existingContributor.SetPhoneNumber(request.NewPhoneNumber);
    }

    await _repository.UpdateAsync(existingContributor, cancellationToken);

    return new ContributorDTO(existingContributor.Id,
      existingContributor.Name, existingContributor.PhoneNumber?.Number ?? "");
  }
}
