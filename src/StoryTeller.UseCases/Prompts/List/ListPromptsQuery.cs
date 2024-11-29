using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.UseCases.Prompts.List;
public record ListPromptsQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<PromptDTO>>>;
