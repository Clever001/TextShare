using TextShareApi.Interfaces.Repositories;

public sealed class TextCleanupService : BackgroundService {
    private readonly ILogger<TextCleanupService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public TextCleanupService(ILogger<TextCleanupService> logger, IServiceScopeFactory scopeFactory) {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInformation("Text cleanup service started.");

        while (!stoppingToken.IsCancellationRequested) {
            try {
                _logger.LogInformation($"{DateTime.Now} : Start of expired texts deletion.");

                using var scope = _scopeFactory.CreateAsyncScope();
                var textRepo = scope.ServiceProvider.GetRequiredService<ITextRepository>();

                var deletionCount = await textRepo.DeleteExpiredTexts();

                _logger.LogInformation($"{DateTime.Now} : Deleted {deletionCount} expired texts.");
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred while cleaning up expired texts.");
            }

            try {
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
            catch (TaskCanceledException) { }
        }

        _logger.LogInformation("Text cleanup service finished its work.");
    }
}