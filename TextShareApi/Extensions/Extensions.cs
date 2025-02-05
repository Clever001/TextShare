using System.Security.Claims;
using TextShareApi.Dtos.Additional;

namespace TextShareApi.Extensions;

public static class Extensions {
    public static ExceptionDto ToExceptionDto(this Exception exception) {
        return new() {
            ExceptionType = exception.GetType().FullName ?? "Exception",
            Message = exception.Message,
        };
    }

    public static string? GetUserName(this ClaimsPrincipal principal) {
        return principal.FindFirst(ClaimTypes.Name)?.Value;
    }
}