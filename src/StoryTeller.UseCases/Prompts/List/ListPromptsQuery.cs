using Ardalis.Result;
using StoryTeller.Core.DTOs;
using MediatR;

namespace StoryTeller.UseCases.Prompts.List;

public record ListPromptsQuery : IRequest<Result<IEnumerable<PromptDTO>>>;
