using StoryTeller.Core.ContributorAggregate;
using StoryTeller.Core.PromptAggregate;

namespace StoryTeller.Infrastructure.Data;

public static class SeedData
{
  public static readonly Contributor Contributor1 = new("Ardalis");
  public static readonly Contributor Contributor2 = new("Snowfrog");
  public static readonly Prompt Prompt1 = new("Sample Prompt 1");
  public static readonly Prompt Prompt2 = new("Sample Prompt 2");

  public static async Task InitializeAsync(AppDbContext dbContext)
  {
    if (await dbContext.Contributors.AnyAsync()) return; // DB has been seeded

    await PopulateTestDataAsync(dbContext);
  }

  public static async Task PopulateTestDataAsync(AppDbContext dbContext)
  {
    dbContext.Contributors.AddRange(new[] { Contributor1, Contributor2 });
    dbContext.Prompts.AddRange(new[] { Prompt1, Prompt2 });
    await dbContext.SaveChangesAsync();
  }
}
