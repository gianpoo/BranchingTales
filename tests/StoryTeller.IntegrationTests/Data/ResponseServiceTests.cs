using StoryTeller.Core.DTOs;
using System.Text.Json;

namespace StoryTeller.IntegrationTests.Data;

/// <summary>
/// Integration tests for the ResponseService implementation.
/// </summary>
public class ResponseServiceTests : BaseTestFixture
{
    public ResponseServiceTests()
    {
        EnsureDataDirectory();
    }

    [Fact]
    public void GetRandomResponse_WhenCalled_ShouldReturnValidJsonWithThreeOptions()
    {
        var response = _responseService.GetRandomResponse();
        Assert.NotNull(response);
        
        var options = JsonSerializer.Deserialize<List<string>>(response);
        Assert.NotNull(options);
        Assert.Equal(3, options.Count);
        Assert.All(options, option => Assert.NotEmpty(option));
    }

    [Fact]
    public async Task SaveChatAsync_WhenCalled_ShouldPersistChat()
    {
        await ResetData();
        await _responseService.SaveChatAsync(1, "Test prompt");
        
        var chats = await _responseService.GetChatsAsync();
        var chat = Assert.Single(chats);
        Assert.Equal(1, chat.Id);
        Assert.Equal("Test prompt", chat.Prompt);
        Assert.True(chat.Timestamp <= DateTime.UtcNow);
    }

    [Fact]
    public async Task GetChatsAsync_WhenNoChatsExist_ShouldReturnEmptyList()
    {
        await ResetData();
        var chats = await _responseService.GetChatsAsync();
        Assert.Empty(chats);
    }

    [Fact]
    public async Task GetChatsAsync_WhenMultipleChatsExist_ShouldReturnAllChats()
    {
        await ResetData();
        var timestamp = DateTime.UtcNow;
        await _responseService.SaveChatAsync(1, "First prompt");
        await _responseService.SaveChatAsync(2, "Second prompt");
        
        var chats = await _responseService.GetChatsAsync();
        Assert.Equal(2, chats.Count);
        Assert.All(chats, chat => 
        {
            Assert.True(chat.Timestamp >= timestamp);
            Assert.NotEmpty(chat.Prompt);
        });
    }

    [Fact]
    public async Task SaveChatAsync_WhenSavingMultipleChats_ShouldMaintainOrder()
    {
        await ResetData();
        await _responseService.SaveChatAsync(1, "First");
        await _responseService.SaveChatAsync(2, "Second");
        await _responseService.SaveChatAsync(3, "Third");
        
        var chats = await _responseService.GetChatsAsync();
        Assert.Equal(3, chats.Count);
        Assert.Equal("First", chats[0].Prompt);
        Assert.Equal("Second", chats[1].Prompt);
        Assert.Equal("Third", chats[2].Prompt);
    }
} 