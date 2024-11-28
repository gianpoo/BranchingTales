namespace StoryTeller.UseCases.Prompts.Create;

public class CreatePromptRequest
{
    public string? Text { get; set; }

    public static string Route => "/Prompts";
} 