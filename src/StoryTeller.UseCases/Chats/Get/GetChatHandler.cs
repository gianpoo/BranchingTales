using System.Collections.Generic;
using System.Linq;
using StoryTeller.Core.ChatAggregate;
using StoryTeller.Core.DTOs;
using StoryTeller.Core.Interfaces;

namespace StoryTeller.UseCases.Chats.Get;

public class GetChatHandler : IQueryHandler<GetChatQuery, Result<ChatDTO>>
{
    private readonly IChatRepository _repository;

    public GetChatHandler(IChatRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ChatDTO>> Handle(GetChatQuery request, CancellationToken cancellationToken)
    {
        var chat = await _repository.GetByIdAsync(1);
        if (chat == null)
        {
            return Result<ChatDTO>.NotFound();
        }

        var prompts = chat.Prompts.Select(p => new PromptDTO(p.Id, p.Text));
        return Result.Success(new ChatDTO(prompts.ToList(), chat.Limit));
    }
} 