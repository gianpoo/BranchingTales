namespace StoryTeller.Infrastructure.Services;
using StoryTeller.Core.Interfaces;
using StoryTeller.Core.DTOs;
using System.Text.Json;

public class ResponseService : IResponseService
{
    private readonly string[] _responses;
    private readonly Random _random = new();
    private readonly string _chatLogPath;
    private readonly string _responsesPath;

    public ResponseService()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        _responsesPath = Path.Combine(baseDir, "Data", "responses.txt");
        _chatLogPath = Path.Combine(baseDir, "Data", "chatlog.json");

        EnsureDirectoryExists();
        EnsureResponsesFileExists();
        EnsureChatLogExists();

        _responses = File.ReadAllLines(_responsesPath);
    }

    private void EnsureDirectoryExists()
    {
        var dataDir = Path.GetDirectoryName(_responsesPath);
        if (!Directory.Exists(dataDir))
        {
            Directory.CreateDirectory(dataDir!);
        }
    }

    private void EnsureResponsesFileExists()
    {
        if (!File.Exists(_responsesPath))
        {
            var defaultResponses = new[]
            {
                "Hello! Let me continue your story...",
                "Here's what happens next in your tale...",
                "The story takes an interesting turn...",
                "Your character encounters a surprising situation...",
                "A mysterious event unfolds...",
                "Suddenly, everything changes when..."
            };
            File.WriteAllLines(_responsesPath, defaultResponses);
        }
    }

    private void EnsureChatLogExists()
    {
        if (!File.Exists(_chatLogPath))
        {
            File.WriteAllText(_chatLogPath, "[]");
        }
    }

    public string GetRandomResponse()
    {
        var options = new List<string>();
        for (int i = 0; i < 3; i++)
        {
            var index = _random.Next(_responses.Length);
            options.Add(_responses[index]);
        }
        return JsonSerializer.Serialize(options);
    }

    public async Task SaveChatAsync(string prompt)
    {
        var chats = await LoadChatsAsync();
        chats.Add(new ChatLogDTO { Id = 1, Prompt = prompt, Timestamp = DateTime.UtcNow });
        await File.WriteAllTextAsync(_chatLogPath, JsonSerializer.Serialize(chats));
    }

    public async Task<List<ChatLogDTO>> GetChatsAsync()
    {
        return await LoadChatsAsync();
    }

    private async Task<List<ChatLogDTO>> LoadChatsAsync()
    {
        var json = await File.ReadAllTextAsync(_chatLogPath);
        return JsonSerializer.Deserialize<List<ChatLogDTO>>(json) ?? new();
    }
} 