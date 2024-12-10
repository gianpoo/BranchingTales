using StoryTeller.Core.ChatAggregate;

namespace StoryTeller.Core.Interfaces;

public interface IChatRepository
{
    Task<Chat> CreateAsync(string initialPrompt, int limit);
    Task<Chat?> GetByIdAsync(int id);
    Task<List<Chat>> GetAllAsync();
    Task AddPromptAsync(string promptText);
    Task<Chat?> GetChat();
    Task SaveChat(Chat chat);
} 