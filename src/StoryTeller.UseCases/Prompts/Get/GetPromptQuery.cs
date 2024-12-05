using StoryTeller.Core.DTOs;

namespace StoryTeller.UseCases.Prompts.Get;

public record GetPromptQuery(int PromptId) : IQuery<Result<PromptDTO>>;