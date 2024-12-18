namespace StoryTeller.Core.Interfaces;

public interface IAIService
{
    Task<string> GenerateStoryResponseAsync(string prompt, int numberOfOptions = 3, CancellationToken cancellationToken = default);
    Task<List<string>> GenerateStoryOptionsAsync(string context, int numberOfOptions = 3, CancellationToken cancellationToken = default);
} 
