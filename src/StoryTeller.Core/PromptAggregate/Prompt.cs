namespace StoryTeller.Core.PromptAggregate;

public class Prompt : EntityBase, IAggregateRoot
{
    public string Text { get; private set; }

    public Prompt(string text)
    {
        Text = Guard.Against.NullOrEmpty(text, nameof(text));
    }
} 