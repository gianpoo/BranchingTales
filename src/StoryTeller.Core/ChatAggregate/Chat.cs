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
    /// Gets the maximum number of iterations allowed with the AI service.
    /// </summary>
    [JsonProperty("limit")]
    public int Limit { get; private set; }

    /// <summary>
    /// Creates a new chat with an initial prompt and interaction limit.
    /// </summary>
    /// <param name="initialPrompt">The text of the first prompt.</param>
    /// <param name="limit">The maximum number of AI service interactions allowed.</param>
    public Chat(string initialPrompt, int limit)
    {
        Guard.Against.NullOrEmpty(initialPrompt);
        Guard.Against.NegativeOrZero(limit, nameof(limit));
        
        Limit = limit;
        AddPrompt(initialPrompt);
    }

    [JsonConstructor]
    private Chat()
    {
        _prompts = new List<Prompt>();
        // No default limit - should only be created through the parameterized constructor
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