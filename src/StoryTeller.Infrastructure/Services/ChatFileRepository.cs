using Newtonsoft.Json;
using StoryTeller.Core.ChatAggregate;
using StoryTeller.Core.Interfaces;
using Ardalis.GuardClauses;

namespace StoryTeller.Infrastructure.Services;

/// <summary>
/// Implements chat storage using a JSON file-based repository pattern.
/// This implementation is thread-safe for concurrent access.
/// </summary>
public class ChatFileRepository : IChatRepository
{
    private readonly string _filePath;
    private static readonly object _lock = new();
    private static readonly JsonSerializerSettings _jsonSettings = new()
    {
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore
    };

    public ChatFileRepository()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        _filePath = Path.Combine(baseDir, "Data", "chats.json");
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
        Guard.Against.NullOrEmpty(initialPrompt);
        var chat = new Chat(initialPrompt);
        
        await Task.Run(() =>
        {
            lock (_lock)
            {
                // Clear existing chats and create new one
                var chats = new List<Chat> { chat };
                var json = JsonConvert.SerializeObject(chats, _jsonSettings);
                File.WriteAllText(_filePath, json);
            }
        });

        return chat;
    }

    public async Task<Chat?> GetByIdAsync(int id)
    {
        return await Task.Run(() =>
        {
            lock (_lock)
            {
                var json = File.ReadAllText(_filePath);
                var chats = JsonConvert.DeserializeObject<List<Chat>>(json, _jsonSettings);
                return chats?.FirstOrDefault();
            }
        });
    }

    public async Task AddPromptAsync(string promptText)
    {
        Guard.Against.NullOrEmpty(promptText);
        await Task.Run(() =>
        {
            lock (_lock)
            {
                // Read existing chats
                var json = File.ReadAllText(_filePath);
                var chats = JsonConvert.DeserializeObject<List<Chat>>(json, _jsonSettings) ?? new List<Chat>();
                
                var chat = chats.FirstOrDefault();
                if (chat == null)
                {
                    // If no chat exists, create one
                    chat = new Chat(promptText);
                    chats = new List<Chat> { chat };
                }
                else
                {
                    // Add prompt to existing chat
                    chat.AddPrompt(promptText);
                }

                // Save the updated chat back to file
                var updatedJson = JsonConvert.SerializeObject(chats, _jsonSettings);
                File.WriteAllText(_filePath, updatedJson);
            }
        });
    }

    public async Task<Chat?> GetChat()
    {
        return await Task.Run(() =>
        {
            lock (_lock)
            {
                var json = File.ReadAllText(_filePath);
                var chats = JsonConvert.DeserializeObject<List<Chat>>(json, _jsonSettings);
                return chats?.FirstOrDefault();
            }
        });
    }

    public async Task SaveChat(Chat chat)
    {
        await Task.Run(() =>
        {
            lock (_lock)
            {
                var json = JsonConvert.SerializeObject(new List<Chat> { chat }, _jsonSettings);
                File.WriteAllText(_filePath, json);
            }
        });
    }

    public async Task<List<Chat>> GetAllAsync()
    {
        return await Task.Run(() =>
        {
            lock (_lock)
            {
                var json = File.ReadAllText(_filePath);
                var chats = JsonConvert.DeserializeObject<List<Chat>>(json, _jsonSettings) ?? new List<Chat>();
                return chats;
            }
        });
    }
} 