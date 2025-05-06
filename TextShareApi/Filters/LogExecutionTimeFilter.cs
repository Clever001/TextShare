using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TextShareApi.Filters;

public class LogExecutionTimeFilter : IActionFilter {
    private readonly ILogger<LogExecutionTimeFilter> _logger;
    private readonly Stopwatch _stopwatch;

    public LogExecutionTimeFilter(ILogger<LogExecutionTimeFilter> logger) {
        _logger = logger;
        _stopwatch = new Stopwatch();
    }
    
    public void OnActionExecuting(ActionExecutingContext context) {
        _stopwatch.Start();
    }

    public void OnActionExecuted(ActionExecutedContext context) {
        _stopwatch.Stop();
        
        var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
        var elapsed = elapsedMilliseconds > 1000
            ? $"{elapsedMilliseconds / 1000} s. {elapsedMilliseconds % 1000} ms."
            : $"{elapsedMilliseconds} ms.";

        var actionName = context.ActionDescriptor.DisplayName;
        var message = $"{actionName} completed in {elapsed}";

        _logger.LogInformation(message);
    }
}