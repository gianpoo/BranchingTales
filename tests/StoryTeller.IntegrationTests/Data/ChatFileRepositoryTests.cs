using StoryTeller.Core.ChatAggregate;

namespace StoryTeller.IntegrationTests.Data;

/// <summary>
/// Integration tests for the ChatFileRepository implementation.
/// </summary>
public class ChatFileRepositoryTests : BaseTestFixture
{
    public ChatFileRepositoryTests()
    {
        EnsureDataDirectory();
    }

    [Fact]
    public async Task CreateAsync_WhenCalled_ShouldCreateNewChatWithCorrectId()
    {
        await ResetData();
        var chat = await _repository.CreateAsync("Test prompt");
        
        Assert.NotEqual(0, chat.Id);
        Assert.Single(chat.Prompts);
        Assert.Equal("Test prompt", chat.Prompts.First().Text);
    }

    [Fact]
    public async Task GetByIdAsync_WhenChatExists_ShouldReturnChat()
    {
        await ResetData();
        var created = await _repository.CreateAsync("Test prompt");
        
        var retrieved = await _repository.GetByIdAsync(created.Id);
        
        Assert.NotNull(retrieved);
        Assert.Equal(created.Id, retrieved.Id);
        Assert.Equal("Test prompt", retrieved.Prompts.First().Text);
    }

    [Fact]
    public async Task GetByIdAsync_WhenChatDoesNotExist_ShouldReturnNull()
    {
        await ResetData();
        var chat = await _repository.GetByIdAsync(999);
        Assert.Null(chat);
    }

    [Fact]
    public async Task GetAllAsync_WhenMultipleChatsExist_ShouldReturnAllChats()
    {
        await ResetData();
        await _repository.CreateAsync("First chat");
        await _repository.CreateAsync("Second chat");
        
        var chats = await _repository.GetAllAsync();
        
        Assert.Equal(2, chats.Count);
        Assert.Equal("First chat", chats[0].Prompts.First().Text);
        Assert.Equal("Second chat", chats[1].Prompts.First().Text);
    }

    [Fact]
    public async Task AddPromptAsync_WhenChatExists_ShouldAddPromptToChat()
    {
        await ResetData();
        var chat = await _repository.CreateAsync("Initial prompt");
        
        await _repository.AddPromptAsync(chat.Id, "Second prompt");
        
        var updated = await _repository.GetByIdAsync(chat.Id);
        Assert.NotNull(updated);
        Assert.Equal(2, updated.Prompts.Count);
        Assert.Equal("Second prompt", updated.Prompts.Last().Text);
    }

    [Fact]
    public async Task AddPromptAsync_WhenChatDoesNotExist_ShouldNotThrowException()
    {
        await ResetData();
        await _repository.AddPromptAsync(999, "Test prompt");
        // Should not throw exception
    }

    [Fact]
    public async Task CreateAsync_WhenCalledMultipleTimes_ShouldAssignUniqueIds()
    {
        await ResetData();
        var chat1 = await _repository.CreateAsync("First");
        var chat2 = await _repository.CreateAsync("Second");
        var chat3 = await _repository.CreateAsync("Third");

        Assert.NotEqual(chat1.Id, chat2.Id);
        Assert.NotEqual(chat2.Id, chat3.Id);
        Assert.NotEqual(chat3.Id, chat1.Id);
    }
} 