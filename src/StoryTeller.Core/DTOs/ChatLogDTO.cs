namespace StoryTeller.Core.DTOs;

public class ChatLogDTO
{
    public int Id { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
} 