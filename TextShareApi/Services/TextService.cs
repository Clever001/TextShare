using TextShareApi.ClassesLib;
using TextShareApi.Dtos.Text;
using TextShareApi.Exceptions;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
using TextShareApi.Models;
using TextShareApi.Models.Enums;

namespace TextShareApi.Services;

public class TextService : ITextService {
    private readonly IAccountRepository _accountRepository;
    private readonly IFriendService _friendService;
    private readonly ILogger<TextService> _logger;
    private readonly ITextRepository _textRepository;
    private readonly ITextSecurityService _textSecurityService;
    private readonly IUniqueIdService _uniqueIdService;

    public TextService(ITextRepository textRepository,
        ITextSecurityService textSecurityService,
        IAccountRepository accountRepository,
        IFriendService friendService,
        IUniqueIdService uniqueIdService,
        ILogger<TextService> logger) {
        _textRepository = textRepository;
        _textSecurityService = textSecurityService;
        _accountRepository = accountRepository;
        _friendService = friendService;
        _uniqueIdService = uniqueIdService;
        _logger = logger;
    }

    public async Task<Result<Text>> Create(string curUserName) {
        var userId = await _accountRepository.GetAccountId(curUserName);
        if (userId == null) return Result<Text>.Failure(new NotFoundException("Current user not found."));

        var text = new Text {
            Id = await _uniqueIdService.GenerateNewHash(),
            AppUserId = userId
        };
        var securitySettings = new TextSecuritySettings {
            TextId = text.Id,
            Text = text,
            AccessType = AccessType.Personal
        };

        await _textRepository.AddText(text, securitySettings);

        return Result<Text>.Success(text);
    }

    public async Task<Result<Text>> GetById(string textId, string? curUserName, string? requestPassword) {
        var text = await _textRepository.GetText(textId);
        if (text == null) return Result<Text>.Failure(new NotFoundException("Text not found."));

        var user = curUserName == null ? null : await _accountRepository.GetAccountByName(curUserName);

        var securityCheckResult = await _textSecurityService.PassReadSecurityChecks(text, user, requestPassword);

        return securityCheckResult.ToGenericResult(text);
    }

    public async Task<Result<List<Text>>> GetAccountTexts(string curUserName) {
        var accountId = await _accountRepository.GetAccountId(curUserName);
        if (accountId == null) return Result<List<Text>>.Failure(new NotFoundException("Current user not found."));

        var texts = await _textRepository.GetTexts(t => t.AppUserId == accountId);
        return Result<List<Text>>.Success(texts);
    }

    public async Task<Result<List<Text>>> GetAllAvailable(string? curUserName) {
        if (curUserName == null) {
            var res = await _textRepository.GetTexts(
                t => t.TextSecuritySettings.AccessType == AccessType.ByReferencePublic, 0, 20);

            return Result<List<Text>>.Success(res);
        }

        var curUserId = await _accountRepository.GetAccountId(curUserName);
        if (curUserId == null) return Result<List<Text>>.Failure(new NotFoundException("Current user not found."));
        var friendsIds = (await _friendService.GetFriendsIds(curUserId)).Value;

        // TODO: Здесь надо будет возвращать dto. Иначе очень большой объем данных.

        var texts = await _textRepository.GetTexts(t =>
                t.AppUserId == curUserId ||
                t.TextSecuritySettings.AccessType == AccessType.ByReferencePublic ||
                t.TextSecuritySettings.AccessType == AccessType.ByReferenceAuthorized ||
                (t.TextSecuritySettings.AccessType == AccessType.OnlyFriends && friendsIds.Contains(t.AppUserId)),
            0, 20
        );

        return Result<List<Text>>.Success(texts);
    }

    public async Task<Result<Text>> Update(string textId, string curUserName, string? requestPassword,
        UpdateTextDto dto) {
        var checkResult = await PerformWriteSecurityChecks(textId, curUserName, requestPassword);
        if (!checkResult.IsSuccess) return checkResult.ToGenericResult(new Text());

        var curUser = await _accountRepository.GetAccountByName(curUserName);

        if (dto is { UpdatePassword: true, Password: not null })
            dto.Password = _textSecurityService.HashPassword(curUser!, dto.Password);

        var text = await _textRepository.UpdateText(textId, dto);
        if (text == null) _logger.LogWarning("Text does not exist. Cannot update");

        return Result<Text>.Success(text);
    }

    public async Task<Result> Delete(string textId, string curUserName, string? requestPassword) {
        var checkResult = await PerformWriteSecurityChecks(textId, curUserName, requestPassword);
        if (!checkResult.IsSuccess) return checkResult;

        var deleted = await _textRepository.DeleteText(textId);
        if (!deleted) _logger.LogWarning("Text does not exist from the beginning. Text not deleted");

        return Result.Success();
    }

    public async Task<Result> Contains(string textId) {
        var exists = await _textRepository.ContainsText(textId);
        if (!exists) {
            return Result.Failure(new NotFoundException("Text does not exist"));
        }
        
        return Result.Success();
    }

    private async Task<Result> PerformWriteSecurityChecks(string textId, string curUserName, string? requestPassword) {
        var text = await _textRepository.GetText(textId);
        if (text == null) return Result.Failure(new NotFoundException("Text not found"));
        var curUser = await _accountRepository.GetAccountByName(curUserName);
        if (curUser == null) return Result.Failure(new NotFoundException("Current user not found"));

        var securityCheck = _textSecurityService.PassWriteSecurityChecks(text, curUser, requestPassword);
        if (!securityCheck.IsSuccess) return Result.Failure(securityCheck.Exception);

        return Result.Success();
    }
}