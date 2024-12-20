namespace StoryTeller.Core.Interfaces;

public interface IAIService
{
    Task<List<string>> GenerateStoryOptionsAsync(
        string context, 
        int currentIteration = 1,
        int totalIterations = 3,
        int numberOfOptions = 3);
} 
