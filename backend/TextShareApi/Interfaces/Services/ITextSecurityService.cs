using Shared;
using TextShareApi.Models;

namespace TextShareApi.Interfaces.Services;

public interface ITextSecurityService {
    Task<Result> PassReadSecurityChecks(Text text, string? requestSenderId, string? password);
    Result PassWriteSecurityChecks(Text text, string requestSenderId);
    string HashPassword(AppUser user, string password);
}