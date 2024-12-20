namespace StoryTeller.Core.Settings;

public class AzureOpenAISettings
{
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string DeploymentName { get; set; } = string.Empty;
    public float Temperature { get; set; } = 0.8f;
    public int MaxTokens { get; set; } = 750;
} 
