using System.Diagnostics;

namespace TextShareApi.ClassesLib;

public class SectionTimer<T> : IDisposable where T : ILogger {
    private readonly T _logger;

    public string? _message;
    private readonly Stopwatch _sw = Stopwatch.StartNew();

    public SectionTimer(T logger) {
        _logger = logger;
    }

    public void Dispose() {
        var ms = _sw.ElapsedMilliseconds;

        var elapsed = ms > 1000 ? $"{ms / 1000} s. {ms % 1000} ms." : $"{ms} ms.";

        if (_message is null) _logger.LogInformation("Ended with user side error. - Elapsed Time: " + elapsed);
        else _logger.LogInformation(_message + " - " + "Elapsed Time: " + elapsed);
    }

    public void SetMessage(string message) {
        _message = message;
    }
}

public static class SectionTimer {
    public static SectionTimer<T> StartNew<T>(T logger) where T : ILogger {
        return new SectionTimer<T>(logger);
    }
}