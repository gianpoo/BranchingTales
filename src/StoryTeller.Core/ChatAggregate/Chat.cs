using Newtonsoft.Json;

namespace StoryTeller.Core.ChatAggregate;

/// <summary>
/// Represents a chat conversation that contains a series of prompts.
/// </summary>
public class Chat
{
    [JsonProperty]
    private readonly List<Prompt> _prompts = new();
    
    /// <summary>
    /// Gets the collection of prompts in this chat.
    /// </summary>
    [JsonProperty("prompts")]
    public IReadOnlyCollection<Prompt> Prompts => _prompts.AsReadOnly();

    /// <summary>
    /// Creates a new chat with an initial prompt.
    /// </summary>
    /// <param name="initialPrompt">The text of the first prompt.</param>
    public Chat(string initialPrompt)
    {
        Guard.Against.NullOrEmpty(initialPrompt);
        AddPrompt(initialPrompt);
    }

    [JsonConstructor]
    public Chat()
    {
        _prompts = new List<Prompt>();
    }

    /// <summary>
    /// Adds a new prompt to this chat.
    /// </summary>
    /// <param name="text">The text of the prompt to add.</param>
    public void AddPrompt(string text)
    {
        Guard.Against.NullOrEmpty(text);
        var promptId = _prompts.Count + 1;
        _prompts.Add(new Prompt(text, promptId));
    }
} 