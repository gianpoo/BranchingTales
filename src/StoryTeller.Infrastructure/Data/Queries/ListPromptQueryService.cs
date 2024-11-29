using StoryTeller.UseCases.Prompts;
using StoryTeller.UseCases.Prompts.List;

namespace StoryTeller.Infrastructure.Data.Queries;

public class ListtPromptsQueryService(AppDbContext _db) : IListPromptsQueryService
{
  // You can use EF, Dapper, SqlClient, etc. for queries -
  // this is just an example

  public async Task<IEnumerable<PromptDTO>> ListAsync()
  {
    // NOTE: This will fail if testing with EF InMemory provider!
    var result = await _db.Database.SqlQuery<PromptDTO>(
      $"SELECT Id, Text FROM Prompts") // don't fetch other big columns
      .ToListAsync();

    return result;
  }
}
