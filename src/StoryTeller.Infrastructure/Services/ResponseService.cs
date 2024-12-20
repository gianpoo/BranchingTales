using System.Text.Json;
using StoryTeller.Core.Interfaces;

namespace StoryTeller.Infrastructure.Services;

public class ResponseService : IResponseService
{
    private readonly string _responsesPath;
    private readonly ILogger<ResponseService> _logger;
    private static readonly object _lock = new();

    public ResponseService(ILogger<ResponseService> logger)
    {
        _logger = logger;
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        _responsesPath = Path.Combine(baseDir, "Data", "options.json");
    }

    private async Task EnsureDirectoryExistsAsync()
    {
        var dataDir = Path.GetDirectoryName(_responsesPath);
        if (!Directory.Exists(dataDir))
        {
            Directory.CreateDirectory(dataDir!);
        }
        if (!File.Exists(_responsesPath))
        {
            await File.WriteAllTextAsync(_responsesPath, JsonSerializer.Serialize(new List<string>()));
        }
    }

    public async Task<List<string>> GetOptionsAsync()
    {
        try
        {
            string json;
            lock (_lock)
            {
                if (!File.Exists(_responsesPath))
                {
                    return new List<string>();
                }
                json = File.ReadAllText(_responsesPath);
            }

            return await Task.Run(() => 
                JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading options");
            return new List<string>();
        }
    }

    public async Task SaveOptionsAsync(List<string> options)
    {
        try
        {
            var json = JsonSerializer.Serialize(options, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });

            await File.WriteAllTextAsync(_responsesPath, json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving options");
            throw;
        }
    }

    public static async Task<ResponseService> CreateAsync(ILogger<ResponseService> logger)
    {
        var service = new ResponseService(logger);
        await service.InitializeAsync();
        return service;
    }

    private async Task InitializeAsync()
    {
        await EnsureDirectoryExistsAsync();
    }
} 
