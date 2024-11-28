namespace StoryTeller.Web.Contributors;

public class SearchContributorRequest
{
    public const string Route = "/Contributors/search/{SearchName}";
    public static string BuildRoute(string searchName) => 
        Route.Replace("{SearchName}", searchName);

    public string SearchName { get; set; } = string.Empty;
} 