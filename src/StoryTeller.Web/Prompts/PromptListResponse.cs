using System.Collections.Generic;
using System.Linq;
using StoryTeller.Core.DTOs;
using StoryTeller.Web.Prompts;

namespace StoryTeller.Web.Prompts;

public class PromptListResponse
{
    public List<PromptRecord> Prompts { get; set; } = new();

    public PromptListResponse(IEnumerable<PromptDTO> prompts)
    {
        Prompts = prompts.Select(p => new PromptRecord(p.Id, p.Text)).ToList();
    }
}
