using StoryTeller.Core.Interfaces;
using StoryTeller.Infrastructure.Services;

namespace StoryTeller.IntegrationTests.Data;

/// <summary>
/// Base test fixture providing common setup for file-based storage tests.
/// </summary>
public abstract class BaseTestFixture
{
    protected readonly IChatRepository _repository;
    protected readonly IResponseService _responseService;

    protected BaseTestFixture()
    {
        _repository = new ChatFileRepository();
        _responseService = new ResponseService();
    }

    /// <summary>
    /// Resets the test data by clearing the storage files.
    /// </summary>
    protected async Task ResetData()
    {
        await File.WriteAllTextAsync("Data/chatlog.json", "[]");
    }

    /// <summary>
    /// Ensures the data directory exists.
    /// </summary>
    protected void EnsureDataDirectory()
    {
        Directory.CreateDirectory("Data");
    }
} 