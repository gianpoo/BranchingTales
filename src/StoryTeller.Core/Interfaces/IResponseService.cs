namespace StoryTeller.Core.Interfaces;

public interface IResponseService
{
    Task<List<string>> GetOptionsAsync();
    Task SaveOptionsAsync(List<string> options);
} 
