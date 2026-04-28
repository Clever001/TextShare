using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using DocShareApi.Dtos.Exception;
using DocShareApi.Exceptions;
using System.IdentityModel.Tokens.Jwt;

namespace DocShareApi.Extensions;

public static class Extensions {
    public static IActionResult ToActionResult(this ControllerBase controller, IApiException exception) {
        return controller.StatusCode(exception.CodeNumber, new ExceptionDto {
            Code = exception.Code,
            Description = exception.Description,
            Details = exception.Details
        });
    }

    public static string? GetId(this ClaimsPrincipal principal) {
        return principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public static string? GetEmail(this ClaimsPrincipal principal) {
        return principal.FindFirstValue(JwtRegisteredClaimNames.Email);
    }

    public static string? GetName(this ClaimsPrincipal principal) {
        return principal.FindFirstValue(ClaimTypes.Name);
    }
}
