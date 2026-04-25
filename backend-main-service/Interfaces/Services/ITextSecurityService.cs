using DocShareApi.ClassesLib;
using DocShareApi.Models;

namespace DocShareApi.Interfaces.Services;

public interface ITextSecurityService {
    Task<Result> PassReadSecurityChecks(Text text, string? requestSenderId, string? password);
    Result PassWriteSecurityChecks(Text text, string requestSenderId);
    Result PassPasswordCheck(Text text, string? password);
    string HashPassword(AppUser user, string password);
}