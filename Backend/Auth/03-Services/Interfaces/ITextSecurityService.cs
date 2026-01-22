using Auth.Models;
using Auth.Other;
using Shared;

namespace Auth.Services.Interfaces;

public interface ITextSecurityService {
    Task<Result> PassReadSecurityChecks(TextSecurityProjection textProjection, string? requestSenderId, string? password);
    Task<Result> PassWriteSecurityChecks(TextSecurityProjection textProjection, string requestSenderId);
    Task<string> HashPassword(string userId, string password);
}