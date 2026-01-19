using Auth.Models;
using Auth.Projections;
using Shared;

namespace Auth.Services.Interfaces;

public interface ITextSecurityService {
    Task<Result> PassReadSecurityChecks(TextSecurityCheckProjection textProjection, string? requestSenderId, string? password);
    Result PassWriteSecurityChecks(TextSecurityCheckProjection textProjection, string requestSenderId);
    string HashPassword(AppUser user, string password);
}