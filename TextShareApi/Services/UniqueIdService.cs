using TextShareApi.Models;

namespace TextShareApi.Services;

public class UniqueIdService : IDisposable {
    private readonly HashSeed _seed;
    private readonly ILogger<UniqueIdService> _logger;
    private readonly object _locker = new();
    public UniqueIdService(ILogger<UniqueIdService> logger)
    {
        _logger = logger;
        _seed = HashGenerator.LoadSeed().Result!;
        if (_seed is null) {
            _seed = new() { NextSeed = 0UL };
            _logger.Log(LogLevel.Warning, "Файл с сидом для генерации хеш-кодов не найден! " +
                                          "Был сгенерирован новый файл с нулевым значением.");
        }
    }

    public async Task<string> GenerateNewHash() {
        UInt64? seed = null;
        lock (_locker) {
            seed = _seed.NextSeed++;
        }
        
        string hash = await HashGenerator.GenerateHash(seed.Value);
        _logger.LogInformation($"Generated new hash: \"{hash}\" for seed: \"{seed}\"");
        
        return hash;
    }

    ~UniqueIdService() {
        ReleaseUnmanagedResources();
    }

    private async void ReleaseUnmanagedResources() {
        await HashGenerator.SaveAsync(_seed);
    }

    public void Dispose() { 
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }
}