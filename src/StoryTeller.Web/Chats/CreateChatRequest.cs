namespace StoryTeller.Web.Chats;

public class CreateChatRequest
{
    public string? Text { get; set; }
    public int Limit { get; set; }  // No default - must be provided in POST
}