namespace StoryTeller.Core.Interfaces;
using StoryTeller.Core.DTOs;

public interface IResponseService
{
    string GetRandomResponse();
    Task SaveChatAsync(string prompt);
    Task<List<ChatLogDTO>> GetChatsAsync();
} 