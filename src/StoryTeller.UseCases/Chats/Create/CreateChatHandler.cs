using StoryTeller.Core.Interfaces;
using StoryTeller.Core.ChatAggregate;

namespace StoryTeller.UseCases.Chats.Create;

public class CreateChatHandler : ICommandHandler<CreateChatCommand, Result>
{
    private readonly IChatRepository _repository;
    private readonly IResponseService _responseService;

    public CreateChatHandler(IChatRepository repository, IResponseService responseService)
    {
        _repository = repository;
        _responseService = responseService;
    }

    public async Task<Result> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        await _repository.CreateAsync(request.Text);
        return Result.Success();
    }
}