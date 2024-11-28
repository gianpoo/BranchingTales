namespace StoryTeller.UseCases.Prompts.Create;

/// <summary>
/// Create a new Prompt.
/// </summary>
/// <param name="Text"></param>
public record CreatePromptCommand(string Text) : Ardalis.SharedKernel.ICommand<Result<int>>; 