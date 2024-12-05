namespace StoryTeller.UseCases.Chats.Create;

public record CreateChatCommand(string Text) : ICommand<Result>;