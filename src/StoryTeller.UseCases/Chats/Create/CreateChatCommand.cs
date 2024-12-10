namespace StoryTeller.UseCases.Chats.Create;

public record CreateChatCommand(string Text, int Limit) : ICommand<Result>;