using TextShareApi.ClassesLib;
using TextShareApi.Models;

namespace TextShareApi.Interfaces.Services;

public interface ITextSecurityService {
    Task<Result> PassReadSecurityChecks(Text text, AppUser? requestSender, string? password);
    Result PassWriteSecurityChecks(Text text, AppUser requestSender, string? password);
    Result PassPasswordCheck(Text text, string? password);
    string HashPassword(AppUser user, string password);
}