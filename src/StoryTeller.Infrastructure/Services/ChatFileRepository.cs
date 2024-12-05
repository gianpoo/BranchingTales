using System.Text.Json;
using System.Text.Json.Serialization;
using StoryTeller.Core.ChatAggregate;
using StoryTeller.Core.Interfaces;

namespace StoryTeller.Infrastructure.Services;

/// <summary>
/// Implements chat storage using a JSON file-based repository pattern.
/// This implementation is thread-safe for concurrent access.
/// </summary>
public class ChatFileRepository : IChatRepository
{
    private readonly string _filePath;
    private static readonly object _lock = new();
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };

    public ChatFileRepository()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        _filePath = Path.Combine(baseDir, "Data", "chatlog.json");
        EnsureFileExists();
    }

    private void EnsureFileExists()
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory!);
        }
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<Chat> CreateAsync(string initialPrompt)
    {
        await File.WriteAllTextAsync(_filePath, "[]");
        
        var chat = new Chat(initialPrompt);
        await SaveChat(chat);
        return chat;
    }

    public async Task<Chat?> GetByIdAsync(int id) => await GetChat();

    public async Task<List<Chat>> GetAllAsync()
    {
        var chat = await GetChat();
        return chat != null ? new List<Chat> { chat } : new List<Chat>();
    }

    public async Task AddPromptAsync(string promptText)
    {
        var chat = await GetChat();
        if (chat == null)
        {
            chat = new Chat(promptText);
        }
        else
        {
            chat.AddPrompt(promptText);
        }
        await SaveChat(chat);
    }

    public async Task<Chat?> GetChat()
    {
        var json = await File.ReadAllTextAsync(_filePath);
        var chats = JsonSerializer.Deserialize<List<Chat>>(json, _jsonOptions);
        return chats?.FirstOrDefault();
    }

    public async Task SaveChat(Chat chat)
    {
        var json = JsonSerializer.Serialize(new List<Chat> { chat }, _jsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }
} 