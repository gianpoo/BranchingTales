namespace StoryTeller.Core.DTOs;

public record ChatDTO(List<PromptDTO> Prompts, int Limit); 