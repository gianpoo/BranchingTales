namespace StoryTeller.UnitTests.UseCases.Contributors;

public class CreateContributorHandlerHandle
{
    private readonly string _testName = "test name";
    private readonly IRepository<Contributor> _repository = Substitute.For<IRepository<Contributor>>();
    private CreateContributorHandler _handler;

    public CreateContributorHandlerHandle()
    {
      _handler = new CreateContributorHandler(_repository);
    }

    private Contributor CreateContributor()
    {
      return new Contributor(_testName); // Using the test name
    }

    [Fact]
    public async Task ReturnsSuccessGivenValidName()
    {
      // Mock the repository to return a Contributor with a generated ID (e.g., 1)
      var contributor = CreateContributor();
      contributor.Id = 1; // Assign a mock ID
      _repository.AddAsync(Arg.Any<Contributor>(), Arg.Any<CancellationToken>())
          .Returns(Task.FromResult(contributor));

      // Execute the handler with the test name
      var result = await _handler.Handle(new CreateContributorCommand(_testName, null), CancellationToken.None);

      // Verify that the result is successful and contains the ID
      result.IsSuccess.Should().BeTrue();
      result.Value.Should().Be(contributor.Id); // Ensure the returned ID is correct
    }

    [Fact]
    public async Task ReturnsFailureGivenInvalidName()
    {
        var result = await _handler.Handle(new CreateContributorCommand("", null), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors.First().Should().Be("Contributor name is required.");
    }
}
