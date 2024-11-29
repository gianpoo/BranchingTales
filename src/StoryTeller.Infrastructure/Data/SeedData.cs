using StoryTeller.Core.PromptAggregate;

namespace StoryTeller.Infrastructure.Data;

public static class SeedData
{
  public static readonly Prompt Prompt1 = new("Sample Prompt 1");
  public static readonly Prompt Prompt2 = new("Sample Prompt 2");

  public static async Task InitializeAsync(AppDbContext dbContext)
  {
    if (await dbContext.Prompts.AnyAsync()) return; // DB has been seeded
    await PopulateTestDataAsync(dbContext);
  }

  public static async Task PopulateTestDataAsync(AppDbContext dbContext)
  {
    dbContext.Prompts.AddRange(new[] { Prompt1, Prompt2 });
    await dbContext.SaveChangesAsync();
  }
}
