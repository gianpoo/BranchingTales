using System.Text.Json.Serialization;

namespace StoryTeller.Core.ChatAggregate;

/// <summary>
/// Represents a single prompt within a chat conversation.
/// </summary>
public class Prompt
{
    /// <summary>
    /// Gets or sets the ID of the prompt.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the text content of the prompt.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Creates a new prompt with the specified text and ID.
    /// </summary>
    /// <param name="text">The text content of the prompt.</param>
    /// <param name="id">The ID of the prompt.</param>
    public Prompt(string text, int id)
    {
        Text = Guard.Against.NullOrEmpty(text);
        Id = Guard.Against.NegativeOrZero(id);
    }

    // Required for serialization
    public Prompt()
    {
    }
}