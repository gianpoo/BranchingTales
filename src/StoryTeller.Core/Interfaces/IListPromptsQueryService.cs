using System.Collections.Generic;
using System.Threading.Tasks;
using StoryTeller.Core.DTOs;

namespace StoryTeller.Core.Interfaces;

public interface IListPromptsQueryService
{
    Task<List<PromptDTO>> ListAsync();
} 