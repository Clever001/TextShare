using System.Linq.Expressions;
using TextShareApi.ClassesLib;
using TextShareApi.Dtos.Enums;
using TextShareApi.Dtos.QueryOptions;
using TextShareApi.Dtos.QueryOptions.Filters;
using TextShareApi.Dtos.Text;
using TextShareApi.Exceptions;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;
using TextShareApi.Models;

namespace TextShareApi.Services;

public class TextService : ITextService {
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<TextService> _logger;
    private readonly ITagRepository _tagRepository;
    private readonly ITextRepository _textRepository;
    private readonly ITextSecurityService _textSecurityService;
    private readonly IUniqueIdService _uniqueIdService;

    public TextService(ITextRepository textRepository,
        ITextSecurityService textSecurityService,
        IAccountRepository accountRepository,
        IUniqueIdService uniqueIdService,
        ILogger<TextService> logger,
        ITagRepository tagRepository) {
        _textRepository = textRepository;
        _textSecurityService = textSecurityService;
        _accountRepository = accountRepository;
        _uniqueIdService = uniqueIdService;
        _logger = logger;
        _tagRepository = tagRepository;
    }

    public async Task<Result<Text>> Create(string curUserName, CreateTextDto dto) {
        if (dto.Title == "") return Result<Text>.Failure(new BadRequestException("Title cannot be empty."));

        if (dto?.Password == "") return Result<Text>.Failure(new BadRequestException("Password cannot be empty."));

        var user = await _accountRepository.GetAccountByName(curUserName);
        var userId = user?.Id;
        if (user == null || userId == null)
            return Result<Text>.Failure(new NotFoundException("Current user not found."));

        var containsText = await _textRepository.ContainsText(dto.Title, userId);
        if (containsText)
            return Result<Text>.Failure(new BadRequestException(
                "Text already exists.",
                ["Text with Composite of fields Title and AppUserId already exists."]
            ));

        var text = new Text {
            Id = await _uniqueIdService.GenerateNewHash(),
            OwnerId = userId,
            Title = dto.Title,
            Description = dto.Description,
            Content = dto.Content,
            Syntax = dto.Syntax,
            Tags = new List<Tag>(),
            ExpiryDate = dto.ExpiryDate
        };
        var securitySettings = new TextSecuritySettings {
            TextId = text.Id,
            Text = text,
            AccessType = dto.AccessType
        };

        if (dto.Password != null) securitySettings.Password = _textSecurityService.HashPassword(user, dto.Password);

        // Adding Tags
        if (dto.Tags.Count > 0) {
            var existingTags = new Dictionary<string, Tag>(
                (await _tagRepository.GetTags(t => dto.Tags.Contains(t.Name))).Select(t =>
                    new KeyValuePair<string, Tag>(t.Name, t)));

            foreach (var tag in dto.Tags.Distinct().Select(t => t.ToLower()))
                text.Tags.Add(existingTags.TryGetValue(tag, out var existingTag)
                    ? existingTag
                    : new Tag { Name = tag });
        }

        await _textRepository.AddText(text, securitySettings);
        text.Owner = new AppUser {
            Id = userId,
            UserName = curUserName
        };
        return Result<Text>.Success(text);
    }

    public async Task<Result<Text>> GetById(string textId, string? curUserName, string? requestPassword) {
        var text = await _textRepository.GetText(textId);
        if (text == null) return Result<Text>.Failure(new NotFoundException("Text not found."));

        var now = DateTime.UtcNow;
        if (now >= text.ExpiryDate) return Result<Text>.Failure(new NotFoundException("Text not found."));

        var userId = curUserName == null ? null : await _accountRepository.GetAccountId(curUserName);

        var securityCheckResult = await _textSecurityService.PassReadSecurityChecks(text, userId, requestPassword);

        if (!securityCheckResult.IsSuccess) return Result<Text>.Failure(securityCheckResult.Exception);

        return Result<Text>.Success(text);
    }

    public async Task<Result<PaginatedResponseDto<Text>>> GetTexts(PaginationDto pagination,
        SortDto sort,
        TextFilterDto filter,
        string? senderName) {
        // Pagination
        var skip = (pagination.PageNumber - 1) * pagination.PageSize;
        var take = pagination.PageSize;

        // Filtering
        var now = DateTime.UtcNow;
        var predicates = new List<Expression<Func<Text, bool>>> {
            t => t.ExpiryDate > now
        };

        if (filter.OwnerName != null)
            predicates.Add(t => t.Owner.UserName!.ToLower().Contains(filter.OwnerName!.ToLower()));

        if (filter.Title != null)
            predicates.Add(t => t.Title.ToLower().Contains(filter.Title.ToLower()));

        if (filter.Tags is { Count: > 0 })
            predicates.Add(text => filter.Tags.All(tag => text.Tags.Any(t => t.Name == tag)));

        if (filter.Syntax != null)
            predicates.Add(text => text.Syntax == filter.Syntax);

        if (filter.AccessType != null)
            predicates.Add(text => text.TextSecuritySettings.AccessType == filter.AccessType);

        if (filter.HasPassword != null)
            predicates.Add(text => text.TextSecuritySettings.Password == null != filter.HasPassword);

        // Security settings check
        if (senderName == null) {
            predicates.Add(text => text.TextSecuritySettings.AccessType == AccessType.ByReferencePublic ||
                                   text.TextSecuritySettings.AccessType == AccessType.ByReferenceAuthorized);
        }
        else {
            var senderId = await _accountRepository.GetAccountId(senderName);
            if (senderId == null)
                return Result<PaginatedResponseDto<Text>>.Failure(new ServerException("Sender not found."));
            predicates.Add(text => text.TextSecuritySettings.AccessType == AccessType.ByReferencePublic ||
                                   text.TextSecuritySettings.AccessType == AccessType.ByReferenceAuthorized ||
                                   (text.TextSecuritySettings.AccessType == AccessType.OnlyFriends &&
                                    (text.OwnerId == senderId || text.Owner.FriendPairs.Select(p => p.SecondUserId)
                                        .Contains(senderId))) ||
                                   (text.TextSecuritySettings.AccessType == AccessType.Personal &&
                                    text.OwnerId == senderId));
        }

        // Sorting and Gathering texts
        var (isValid, getTexts) = GenerateGetTextsFunc(sort.SortBy);
        if (!isValid)
            return Result<PaginatedResponseDto<Text>>.Failure(new BadRequestException("Invalid Sort By field."));
        var (count, texts) = await getTexts(skip, take, sort.SortAscending, predicates);

        return Result<PaginatedResponseDto<Text>>.Success(texts.ToPaginatedResponse(pagination, count));
    }

    public async Task<Result<List<Text>>> GetLatestTexts() {
        var now = DateTime.UtcNow;
        var predicates = new List<Expression<Func<Text, bool>>> {
            t => t.ExpiryDate > now,
            t => t.TextSecuritySettings.AccessType == AccessType.ByReferencePublic ||
                 t.TextSecuritySettings.AccessType == AccessType.ByReferenceAuthorized,
            t => t.TextSecuritySettings.Password == null
        };

        var (_, texts) = await _textRepository.GetTexts(
            0,
            5,
            t => t.CreatedOn,
            false,
            predicates,
            false
        );

        return Result<List<Text>>.Success(texts);
    }

    public async Task<Result<Text>> Update(string textId, string curUserName,
        UpdateTextDto dto) {
        var text = await _textRepository.GetText(textId);
        if (text == null) return Result<Text>.Failure(new NotFoundException("Text not found."));

        var now = DateTime.UtcNow;
        if (now >= text.ExpiryDate) return Result<Text>.Failure(new NotFoundException("Text not found."));

        // Security check
        var curUserId = await _accountRepository.GetAccountId(curUserName);
        if (curUserId == null) return Result<Text>.Failure(new ServerException("Current user not found."));
        var securityCheck = _textSecurityService.PassWriteSecurityChecks(text, curUserId);
        if (!securityCheck.IsSuccess) return Result<Text>.Failure(securityCheck.Exception);

        // Title existence check
        if (dto.Title != null) {
            var contains = await _textRepository.ContainsText(dto.Title, curUserId);
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
        if (dto.Tags != null) {
            var existingTags = new Dictionary<string, Tag>(
                (await _tagRepository.GetTags(t => dto.Tags.Contains(t.Name))).Select(t =>
                    new KeyValuePair<string, Tag>(t.Name, t)));
            text.Tags.Clear();
            foreach (var tag in dto.Tags.Distinct().Select(t => t.ToLower()))
                text.Tags.Add(existingTags.TryGetValue(tag, out var existingTag)
                    ? existingTag
                    : new Tag { Name = tag });
        }

        if (dto.ExpiryDate.HasValue) text.ExpiryDate = dto.ExpiryDate.Value;

        if (dto.AccessType != null) text.TextSecuritySettings.AccessType = dto.AccessType.Value;
        if (dto.UpdatePassword) text.TextSecuritySettings.Password = dto.Password;

        text.UpdatedOn = DateTime.UtcNow;

        await _textRepository.UpdateText(text);

        return Result<Text>.Success(text);
    }

    public async Task<Result> Delete(string textId, string curUserName) {
        var text = await _textRepository.GetText(textId);
        if (text == null) return Result.Failure(new NotFoundException("Text not found."));

        var now = DateTime.UtcNow;
        if (now >= text.ExpiryDate) return Result.Failure(new NotFoundException("Text not found."));


        // Security check
        var curUserId = await _accountRepository.GetAccountId(curUserName);
        if (curUserId == null) return Result.Failure(new ServerException("Current user not found."));
        var securityCheck = _textSecurityService.PassWriteSecurityChecks(text, curUserId);
        if (!securityCheck.IsSuccess) return Result.Failure(securityCheck.Exception);


        // Deleting text
        var deleted = await _textRepository.DeleteText(textId);
        if (!deleted) {
            // Never executed. Text existence check was performed previously.
            _logger.LogWarning("Text did not exist from the beginning. Text not deleted.");
            return Result.Failure(new ServerException());
        }

        return Result.Success();
    }

    public async Task<Result<PaginatedResponseDto<Text>>> GetTextsByName(PaginationDto pagination,
        SortDto sort,
        TextFilterWithoutOwnerDto filter,
        string ownerName,
        string? senderName) {
        var accountExists = await _accountRepository.ContainsAccountByName(ownerName);
        if (!accountExists)
            return Result<PaginatedResponseDto<Text>>.Failure(
                new NotFoundException("Account with this name was not found."));

        var convertedFilter = new TextFilterDto {
            OwnerName = ownerName,
            Title = filter.Title,
            Tags = filter.Tags,
            Syntax = filter.Syntax,
            AccessType = filter.AccessType,
            HasPassword = filter.HasPassword
        };

        return await GetTexts(pagination, sort, convertedFilter, senderName);
    }

    private (bool isValid, Func<int, int, bool, List<Expression<Func<Text, bool>>>, Task<(int, List<Text>)>>)
        GenerateGetTextsFunc(string sortBy) {
        return sortBy.ToLower() switch {
            "id" => (true, (skip, take, asc, predicates) =>
                _textRepository.GetTexts(skip, take, t => t.Id, asc, predicates, true)),
            "title" => (true, (skip, take, asc, predicates) =>
                _textRepository.GetTexts(skip, take, t => t.Title, asc, predicates, true)),
            "syntax" => (true, (skip, take, asc, predicates) =>
                _textRepository.GetTexts(skip, take, t => t.Syntax, asc, predicates, true)),
            "createdon" => (true, (skip, take, asc, predicates) =>
                _textRepository.GetTexts(skip, take, t => t.CreatedOn, asc, predicates, true)),
            "updatedon" => (true, (skip, take, asc, predicates) =>
                _textRepository.GetTexts(skip, take, t => t.UpdatedOn, asc, predicates, true)),
            _ => (false, null!)
        };
    }
}