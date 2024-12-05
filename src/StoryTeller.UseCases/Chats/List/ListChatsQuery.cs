using System.Collections.Generic;
using StoryTeller.Core.DTOs;
using Ardalis.SharedKernel;

namespace StoryTeller.UseCases.Chats.List;

public record ListChatsQuery : IQuery<Result<List<ChatDTO>>>; 