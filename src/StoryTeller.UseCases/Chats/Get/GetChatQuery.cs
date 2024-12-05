using StoryTeller.Core.DTOs;
using StoryTeller.Core.Interfaces;

namespace StoryTeller.UseCases.Chats.Get;

public record GetChatQuery(int ChatId) : IQuery<Result<ChatDTO>>; 