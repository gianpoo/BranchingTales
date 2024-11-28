using StoryTeller.Core.PromptAggregate;

namespace StoryTeller.UseCases.Prompts.Create;

public class CreatePromptHandler : ICommandHandler<CreatePromptCommand, Result<int>>
{
    private readonly IRepository<Prompt> _repository;

    public CreatePromptHandler(IRepository<Prompt> repository)
    {
        _repository = repository;
    }

    public async Task<Result<int>> Handle(CreatePromptCommand request, CancellationToken cancellationToken)
    {
        // Validate input
        if (string.IsNullOrEmpty(request.Text))
        {
            return Result<int>.Error("Text is required.");
        }

        // Create a new Prompt instance
        var newPrompt = new Prompt(request.Text);

        // Add the prompt to the repository
        var createdItem = await _repository.AddAsync(newPrompt, cancellationToken);

        // Return success with the created prompt's ID
        return Result<int>.Success(createdItem.Id);
    }
} 