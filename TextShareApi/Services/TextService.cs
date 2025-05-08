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

    public async Task<Result<Text>> Create(string curUserName, CreateTextDto dto) {
        if (dto.Title == "") {
            return Result<Text>.Failure(new BadRequestException("Title cannot be empty."));
        }

        if (dto?.Password == "") {
            return Result<Text>.Failure(new BadRequestException("Password cannot be empty."));
        }
        
        var userId = await _accountRepository.GetAccountId(curUserName);
        if (userId == null) return Result<Text>.Failure(new NotFoundException("Current user not found."));

        bool containsText = await _textRepository.ContainsText(dto.Title, userId);
        if (containsText)
            return Result<Text>.Failure(new BadRequestException(
                description: "Text already exists.",
                details: ["Text with Composite of fields Title and AppUserId already exists."]
            ));

        var text = new Text {
            Id = await _uniqueIdService.GenerateNewHash(),
            OwnerId = userId,
            Title = dto.Title,
            Description = dto.Description,
            Content = dto.Content,
            Syntax = dto.Syntax,
        };
        var securitySettings = new TextSecuritySettings {
            TextId = text.Id,
            Text = text,
            AccessType = dto.AccessType,
            Password = dto.Password,
        };
        await _textRepository.AddText(text, securitySettings);
        return Result<Text>.Success(text);
    }

    public async Task<Result<Text>> GetById(string textId, string? curUserName, string? requestPassword) {
        var text = await _textRepository.GetText(textId);
        if (text == null) return Result<Text>.Failure(new NotFoundException("Text not found."));

        var userId = curUserName == null ? null : await _accountRepository.GetAccountId(curUserName);

        var securityCheckResult = await _textSecurityService.PassReadSecurityChecks(text, userId, requestPassword);

        if (!securityCheckResult.IsSuccess) {
            return Result<Text>.Failure(securityCheckResult.Exception);
        }

        return Result<Text>.Success(text);
    }

    public async Task<Result<List<Text>>> GetAccountTexts(string curUserName) {
        var accountId = await _accountRepository.GetAccountId(curUserName);
        if (accountId == null) return Result<List<Text>>.Failure(new NotFoundException("Current user not found."));

        var texts = await _textRepository.GetTexts(t => t.OwnerId == accountId);
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
                t.OwnerId == curUserId ||
                t.TextSecuritySettings.AccessType == AccessType.ByReferencePublic ||
                t.TextSecuritySettings.AccessType == AccessType.ByReferenceAuthorized ||
                (t.TextSecuritySettings.AccessType == AccessType.OnlyFriends && friendsIds.Contains(t.OwnerId)),
            0, 20
        );

        return Result<List<Text>>.Success(texts);
    }

    public async Task<Result<Text>> Update(string textId, string curUserName, string? requestPassword,
        UpdateTextDto dto) {
        var text = await _textRepository.GetText(textId);
        if (text == null) return Result<Text>.Failure(new NotFoundException("Text not found."));
        
        
        // Security check
        var curUserId = await _accountRepository.GetAccountId(curUserName);
        if (curUserId == null) return Result<Text>.Failure(new ServerException("Current user not found."));
        var securityCheck = _textSecurityService.PassWriteSecurityChecks(text, curUserId);
        if (!securityCheck.IsSuccess) return Result<Text>.Failure(securityCheck.Exception);
        
        // Title existence check
        if (dto.Title != null) {
            bool contains = await _textRepository.ContainsText(dto.Title, curUserId);
            if (contains) return Result<Text>.Failure(new BadRequestException("This Title already exists."));
        }
        
        // Hashing Password
        if (dto is { UpdatePassword: true, Password: not null }) {
            var curUser = await _accountRepository.GetAccountByName(curUserName);
            dto.Password = _textSecurityService.HashPassword(curUser!, dto.Password);
        }
        
        
        // Updating Text
        if (dto.Content != null) text.Content = dto.Content;
        if (dto.Title != null) text.Title = dto.Title;
        if (dto.Description != null) text.Description = dto.Description;
        if (dto.Syntax != null) text.Syntax = dto.Syntax;
        if (dto.AccessType != null) text.TextSecuritySettings.AccessType = dto.AccessType.Value;
        if (dto.UpdatePassword) text.TextSecuritySettings.Password = dto.Password;

        text.UpdatedOn = DateTime.UtcNow;

        await _textRepository.UpdateText(text);

        return Result<Text>.Success(text);
    }

    public async Task<Result> Delete(string textId, string curUserName, string? requestPassword) {
        var text = await _textRepository.GetText(textId);
        if (text == null) return Result.Failure(new NotFoundException("Text not found."));
        
        
        // Security check
        var curUserId = await _accountRepository.GetAccountId(curUserName);
        if (curUserId == null) return Result.Failure(new ServerException("Current user not found."));
        var securityCheck = _textSecurityService.PassWriteSecurityChecks(text, curUserId);
        if (!securityCheck.IsSuccess) return Result.Failure(securityCheck.Exception);

        
        // Deleting text
        var deleted = await _textRepository.DeleteText(textId);
        if (!deleted) {
            // Never executed. Text existence was performed previously.
            _logger.LogWarning("Text did not exist from the beginning. Text not deleted.");
            return Result.Failure(new ServerException());
        }

        return Result.Success();
    }

    public async Task<Result> Contains(string textId) {
        var exists = await _textRepository.ContainsText(textId);
        if (!exists) {
            return Result.Failure(new NotFoundException("Text not found."));
        }
        
        return Result.Success();
    }
}