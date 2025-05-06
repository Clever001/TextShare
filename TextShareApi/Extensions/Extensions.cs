using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TextShareApi.ClassesLib;
using TextShareApi.Dtos.Additional;

namespace TextShareApi.Extensions;

public static class Extensions {
    public static ExceptionDto ToExceptionDto(this Exception exception) {
        return new ExceptionDto {
            Code = exception.GetType().FullName ?? exception.GetType().Name,
            Description = exception.Message
        };
    }

    public static ExceptionDto ToExceptionDto(this ModelStateDictionary modelStateDictionary) {
        List<string> errors = new();

        foreach (var (key, value) in modelStateDictionary) {
            foreach (var error in value.Errors) {
                errors.Add(error.ErrorMessage);
            }
        }

        return new ExceptionDto {
            Code = "ValidationFailed",
            Description = "One or more validation errors occurred.",
            Details = errors
        };
    }

    public static ExceptionDto ToExceptionDto<T>(this Result<T> result) {
        return new ExceptionDto {
            Code = "Unknown because of bad architecture",
            Description = result.Error
        };
    }

    public static string? GetUserName(this ClaimsPrincipal principal) {
        return principal.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.GivenName))?.Value;
    }
}