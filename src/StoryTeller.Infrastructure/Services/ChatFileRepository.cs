using Newtonsoft.Json;
using StoryTeller.Core.ChatAggregate;
using StoryTeller.Core.Interfaces;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;

namespace StoryTeller.Infrastructure.Services;

public class ChatFileRepository : IChatRepository
{
  private readonly string _filePath;
  private static readonly object _lock = new();
  private static readonly JsonSerializerSettings _jsonSettings = new()
  {
    Formatting = Formatting.Indented,
    NullValueHandling = NullValueHandling.Ignore
  };
  private readonly ILogger<ChatFileRepository> _logger;

  public ChatFileRepository(ILogger<ChatFileRepository> logger)
  {
    var baseDir = AppDomain.CurrentDomain.BaseDirectory;
    _filePath = Path.Combine(baseDir, "Data", "chats.json");
    _logger = logger;
  }

  private async Task EnsureFileExistsAsync()
  {
    var directory = Path.GetDirectoryName(_filePath);
    if (!Directory.Exists(directory))
    {
      Directory.CreateDirectory(directory!);
    }
    if (!File.Exists(_filePath))
    {
      await File.WriteAllTextAsync(_filePath, "[]");
    }
  }

  public static async Task<ChatFileRepository> CreateAsync(ILogger<ChatFileRepository> logger)
  {
    var repo = new ChatFileRepository(logger);
    await repo.InitializeAsync();
    return repo;
  }

  private async Task InitializeAsync()
  {
    await EnsureFileExistsAsync();
  }

  public async Task<Chat> CreateAsync(string initialPrompt, int limit)
  {
    Guard.Against.NullOrEmpty(initialPrompt);
    Guard.Against.NegativeOrZero(limit);

    var chat = new Chat(initialPrompt, limit);
    
    // Read existing chats
    List<Chat> existingChats;
    try
    {
      string json = await File.ReadAllTextAsync(_filePath);
      existingChats = JsonConvert.DeserializeObject<List<Chat>>(json, _jsonSettings) ?? new List<Chat>();
    }
    catch
    {
      existingChats = new List<Chat>();
    }

    // Add new chat
    existingChats.Add(chat);

    // Save all chats
    var updatedJson = JsonConvert.SerializeObject(existingChats, _jsonSettings);
    await File.WriteAllTextAsync(_filePath, updatedJson);

    return chat;
  }

  public async Task<Chat?> GetByIdAsync(int id)
  {
    try
    {
      string json;
      lock (_lock)
      {
        json = File.ReadAllText(_filePath);
      }

      return await Task.Run(() =>
      {
        var chats = JsonConvert.DeserializeObject<List<Chat>>(json, _jsonSettings);
        return chats?.FirstOrDefault();
      });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error getting chat by id {Id}", id);
      return null;
    }
  }

  public async Task AddPromptAsync(string promptText)
  {
    try
    {
      var chat = await GetChat();
      if (chat == null)
      {
        _logger.LogError("No chat found to add prompt to");
        throw new InvalidOperationException("No chat found");
      }

      chat.AddPrompt(promptText);
      await SaveChat(chat);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error adding prompt: {PromptText}", promptText);
      throw;
    }
  }

  public async Task<Chat?> GetChat()
  {
    try
    {
      var chats = await GetAllAsync();
      return chats.FirstOrDefault();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error getting chat");
      return null;
    }
  }

  public async Task SaveChat(Chat chat)
  {
    try
    {
      var chats = await GetAllAsync();
      var existingChat = chats.FirstOrDefault();
      if (existingChat != null)
      {
        // Replace existing chat
        chats[0] = chat;
      }
      else
      {
        // Add new chat
        chats.Add(chat);
      }

      var json = JsonConvert.SerializeObject(chats, _jsonSettings);
      await File.WriteAllTextAsync(_filePath, json);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error saving chat");
      throw;
    }
  }

  public async Task<List<Chat>> GetAllAsync()
  {
    try
    {
      string json;
      lock (_lock)
      {
        json = File.ReadAllText(_filePath);
      }

      return await Task.Run(() =>
          JsonConvert.DeserializeObject<List<Chat>>(json, _jsonSettings) ?? new List<Chat>());
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error getting all chats");
      return new List<Chat>();
    }
  }
}
