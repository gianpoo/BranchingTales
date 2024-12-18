namespace StoryTeller.Core.Settings;

public class AzureOpenAISettings
{
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string DeploymentName { get; set; } = string.Empty;
    public float Temperature { get; set; } = 0.7f;
    public int MaxTokens { get; set; } = 800;
    public float TopP { get; set; } = 0.95f;
    public float FrequencyPenalty { get; set; } = 0;
    public float PresencePenalty { get; set; } = 0;
} 
