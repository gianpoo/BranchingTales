namespace StoryTeller.UseCases.Prompts.Create;

public record CreatePromptCommand(string Text) : ICommand<Result<string>>;