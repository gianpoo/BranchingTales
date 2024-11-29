namespace StoryTeller.UseCases.Prompts.List;
public interface IListPromptsQueryService
{
  Task<IEnumerable<PromptDTO>> ListAsync();
}
