namespace StoryTeller.Core.ContributorAggregate.Specifications;
using Microsoft.EntityFrameworkCore;

public class ContributorByNameSpec : Specification<Contributor>
{
    public ContributorByNameSpec(string searchTerm)
    {
        if (searchTerm.Length < 3)
        {
            Query.Where(c => false);
            return;
        }

        var escapedTerm = searchTerm.Replace("[", "[[]")
                                  .Replace("%", "[%]")
                                  .Replace("_", "[_]")
                                  .ToLower();
                                  
        Query.Where(c => EF.Functions.Like(c.Name.ToLower(), $"%{escapedTerm}%"));
    }
} 