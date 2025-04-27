using System.Security.Claims;
using TextShareApi.Dtos.Additional;

namespace TextShareApi.Extensions;

public static class Extensions {
    public static ExceptionDto ToExceptionDto(this Exception exception) {
        return new ExceptionDto {
            Code = exception.GetType().FullName ?? exception.GetType().Name,
            Description = exception.Message
        };
    }

    public static string? GetUserName(this ClaimsPrincipal principal) {
        return principal.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.GivenName))?.Value;
    }
}