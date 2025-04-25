using System.Linq.Expressions;
using TextShareApi.ClassesLib;
using TextShareApi.Data;
using TextShareApi.Dtos.Text;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
using TextShareApi.Models;
using TextShareApi.Models.Enums;

namespace TextShareApi.Services;

public class TextService : ITextService {
    private readonly ITextRepository _textRepository;
    private readonly ITextSecurityService _textSecurityService;
    private readonly IAccountRepository _accountRepository;
    private readonly IFriendService _friendService;
    private readonly IUniqueIdService _uniqueIdService;

    public TextService(ITextRepository textRepository,
        ITextSecurityService textSecurityService,
        IAccountRepository accountRepository,
        IFriendService friendService,
        IUniqueIdService uniqueIdService) {
        _textRepository = textRepository;
        _textSecurityService = textSecurityService;
        _accountRepository = accountRepository;
        _friendService = friendService;
        _uniqueIdService = uniqueIdService;
    }
    
    public async Task<Result<Text>> Create(string curUserName) {
        var userId = await _accountRepository.GetAccountId(curUserName);
        if (userId == null) {
            return Result<Text>.Failure("Current user not found", false);
        }

        var text = new Text {
            Id = await _uniqueIdService.GenerateNewHash(),
            AppUserId = userId,
        };
        var securitySettings = new TextSecuritySettings {
            TextId = text.Id,
            Text = text,
            AccessType = AccessType.Personal,
            Password = string.Empty,
        };
        
        await _textRepository.AddText(text, securitySettings);
        
        return Result<Text>.Success(text);
    }

    public async Task<Result<Text>> GetById(string id, string? curUserName, string? requestPassword) {
        var text = await _textRepository.GetText(id);
        if (text == null) {
            return Result<Text>.Failure("Text not found", false);
        }

        var user = await _accountRepository.GetAccount(id);
        
        var securityCheckResult = await _textSecurityService.PassSecurityChecks(text, user, requestPassword);
        
        return securityCheckResult.ToGenericResult(text);
    }

    public async Task<Result<List<Text>>> GetAccountTexts(string curUserName) {
        var accountId = await _accountRepository.GetAccountId(curUserName);
        if (accountId == null) {
            return Result<List<Text>>.Failure("Current user not found", false);
        }

        var texts = await _textRepository.GetTexts(t => t.AppUserId == accountId);
        return Result<List<Text>>.Success(texts);
    }

    public async Task<Result<List<Text>>> GetAllAvailable(string? curUserName) {
        if (curUserName == null) {
            var res = await _textRepository.GetTexts(
                t => t.TextSecuritySettings.AccessType == AccessType.ByReferencePublic, 0, 20);
            
            return Result<List<Text>>.Success(res);
        }
        
        string? curUserId = await _accountRepository.GetAccountId(curUserName);
        if (curUserId == null) return Result<List<Text>>.Failure("Current user not found", false);
        List<string> friendsIds = (await _friendService.GetFriendsIds(curUserId)).Value;
        
        // TODO: Здесь надо будет возвращать dto. Иначе очень большой объем данных.
        
        var texts = await _textRepository.GetTexts(t => 
            t.AppUserId == curUserId || 
            t.TextSecuritySettings.AccessType == AccessType.ByReferencePublic ||
            t.TextSecuritySettings.AccessType == AccessType.ByReferenceAuthorized ||
            t.TextSecuritySettings.AccessType == AccessType.OnlyFriends && friendsIds.Contains(t.AppUserId),
            skipCnt: 0, maxCnt: 20
            );

        return Result<List<Text>>.Success(texts);
    }

    public async Task<Result<Text>> Update(string id, UpdateTextDto dto) {
        
    }

    public async Task<Result> Delete(string id) {
        throw new NotImplementedException();
    }
}