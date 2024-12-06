using StoryTeller.Core.DTOs;

namespace StoryTeller.UseCases.Chats.GetPrompts;

public record GetChatPromptsQuery(int ChatId) : IQuery<Result<List<PromptDTO>>>; 
