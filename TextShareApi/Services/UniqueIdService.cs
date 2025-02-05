using TextShareApi.Models;

namespace TextShareApi.Services;

public class UniqueIdService : IDisposable {
    private readonly HashSeed _seed = null!;
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

    /*public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }*/

    public async Task<string> GenerateNewHash() {
        UInt64? seed = null;
        lock (_locker) {
            seed = _seed.NextSeed++;
        }
        
        return await HashGenerator.GenerateHash(seed.Value);
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