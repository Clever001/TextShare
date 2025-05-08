using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.Dtos.Exception;
using TextShareApi.Exceptions;

namespace TextShareApi.Extensions;

public static class Extensions {
    public static IActionResult ToActionResult(this ControllerBase controller, IApiException exception) {
        return controller.StatusCode(exception.CodeNumber, new ExceptionDto {
            Code = exception.Code,
            Description = exception.Description,
            Details = exception.Details
        });
    }
    
    public static string? GetUserName(this ClaimsPrincipal principal) {
        return principal.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.GivenName))?.Value;
    }
}