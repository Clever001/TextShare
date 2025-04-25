using TextShareApi.ClassesLib;
using TextShareApi.Models;
using TextShareApi.Models.Enums;

namespace TextShareApi.Interfaces.Services;

public interface ITextSecurityService {
    Task<Result> PassSecurityChecks(Text text, AppUser? appUser, string? password);
}