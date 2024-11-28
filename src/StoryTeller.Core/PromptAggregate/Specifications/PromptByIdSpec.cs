namespace StoryTeller.Core.PromptAggregate.Specifications;

public class PromptByIdSpec : Specification<Prompt>
{
    public PromptByIdSpec(int promptId)
    {
        Query.Where(prompt => prompt.Id == promptId);
    }
} 