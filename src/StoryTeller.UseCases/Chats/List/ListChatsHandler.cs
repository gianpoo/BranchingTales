using System.Collections.Generic;
using System.Linq;
using StoryTeller.Core.ChatAggregate;
using StoryTeller.Core.DTOs;
using StoryTeller.Core.Interfaces;
using Ardalis.SharedKernel;

namespace StoryTeller.UseCases.Chats.List;

public class ListChatsHandler : IQueryHandler<ListChatsQuery, Result<List<ChatDTO>>>
{
    private readonly IChatRepository _repository;

    public ListChatsHandler(IChatRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<ChatDTO>>> Handle(ListChatsQuery request, CancellationToken cancellationToken)
    {
        var chats = await _repository.GetAllAsync();
        var chatDtos = chats.Select(c => new ChatDTO(
            c.Prompts.Select(p => new PromptDTO(p.Id, p.Text)).ToList(),
            c.Limit)).ToList();
        return Result<List<ChatDTO>>.Success(chatDtos);
    }
} 