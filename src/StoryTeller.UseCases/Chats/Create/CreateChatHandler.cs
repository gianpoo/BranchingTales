using StoryTeller.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace StoryTeller.UseCases.Chats.Create;

public class CreateChatHandler : ICommandHandler<CreateChatCommand, Result>
{
    private readonly IChatRepository _repository;
    private readonly IAIService _aiService;
    private readonly IResponseService _responseService;
    private readonly ILogger<CreateChatHandler> _logger;

    public CreateChatHandler(
        IChatRepository repository,
        IAIService aiService,
        IResponseService responseService,
        ILogger<CreateChatHandler> logger)
    {
        _repository = repository;
        _aiService = aiService;
        _responseService = responseService;
        _logger = logger;
    }

    public async Task<Result> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Create new chat
            var chat = await _repository.CreateAsync(request.Text, request.Limit);

            // Generate and save initial options
            var options = await _aiService.GenerateStoryOptionsAsync(
                request.Text,
                currentIteration: 1,
                totalIterations: request.Limit);
            
            await _responseService.SaveOptionsAsync(options);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating chat");
            return Result.Error("Failed to create chat");
        }
    }
}
