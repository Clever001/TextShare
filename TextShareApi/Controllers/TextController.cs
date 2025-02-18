using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TextShareApi.Dtos.Text;
using TextShareApi.Extensions;
using TextShareApi.Interfaces;
using TextShareApi.Mappers;
using TextShareApi.Models;
using TextShareApi.Models.Enums;
using TextShareApi.Services;

namespace TextShareApi.Controllers;

[Route("api/text")]
[ApiController]
public class TextController : ControllerBase {
    private readonly ITextRepository _textRepository;
    private readonly ITextSecuritySettingsRepository _securitySettingsRepository;
    private readonly IUniqueIdService _uniqueIdService;
    private readonly IFriendsRepository _friendsRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly PasswordHasher<AppUser> _passwordHasher;

    public TextController(ITextRepository textRepository,
        ITextSecuritySettingsRepository securitySettingsRepository,
        IUniqueIdService uniqueIdService,
        IFriendsRepository friendsRepository,
        UserManager<AppUser> userManager,
        PasswordHasher<AppUser> passwordHasher) {
        _textRepository = textRepository;
        _securitySettingsRepository = securitySettingsRepository;
        _uniqueIdService = uniqueIdService;
        _friendsRepository = friendsRepository;
        _userManager = userManager;
        _passwordHasher = passwordHasher;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateTextDto createTextDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        string userName = User.GetUserName();
        AppUser? creatorUser = await _userManager.FindByNameAsync(userName);
        if (creatorUser is null) {
            return StatusCode(500, "User does not exist");
        }
        
        var text = new Text {
            Id = await _uniqueIdService.GenerateNewHash(),
            Content = createTextDto.Text ?? string.Empty,
            AppUserId = creatorUser.Id,
        };
        AccessType accessType;
        if (createTextDto.Personal) accessType = AccessType.Personal;
        else if (createTextDto.ByReferencePublic) accessType = AccessType.ByReferencePublic;
        else if (createTextDto.ByReferenceAuthorized) accessType = AccessType.ByReferenceAuthorized;
        else accessType = AccessType.OnlyFriends;
        var textSettings = new TextSecuritySettings {
            TextId = text.Id,
            AccessType = accessType,
            Password = createTextDto.Password is null ? null : _passwordHasher.HashPassword(creatorUser, createTextDto.Password),
        };
        text = await _textRepository.CreateText(text, textSettings);
        
        return Ok(text.ToTextDto(userName));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id, [FromQuery] string? requestPassword) {
        IActionResult noAccess = BadRequest("User does not have access to this text");

        string senderName = User.GetUserName();
        AppUser? sender = await _userManager.FindByNameAsync(senderName);

        var text = await _textRepository.GetTextWithBackground(id);
        if (text is null) return NotFound("Text not found");

        if (sender is not null && sender.Id == text.AppUserId) {
            return Ok(text.ToTextDto()); // Owner always has rights.
        }
        
        var textSecuritySettings = text.TextSecuritySettings;
        switch (textSecuritySettings.AccessType) {
            case AccessType.Personal: {
                if (sender is null) return noAccess;
                if (sender.Id != text.AppUserId) return noAccess;
                break;
            }
            case AccessType.ByReferencePublic: {
                break;
            }
            case AccessType.ByReferenceAuthorized: {
                if (sender is null) return noAccess;
                break;
            }
            case AccessType.OnlyFriends: {
                if (sender is null) return noAccess;
                var ownerId = text.AppUserId;
                var friendsIds = await _friendsRepository.GetFriendsIds(ownerId);
                if (!friendsIds.Contains(sender.Id)) return noAccess;
                break;
            }
        }

        if (textSecuritySettings.Password is not null) {
            if (requestPassword is null) return BadRequest("Provide password");
            var result = _passwordHasher.VerifyHashedPassword(text.AppUser, textSecuritySettings.Password,
                requestPassword);
            if (result == PasswordVerificationResult.Failed) {
                return BadRequest("Password is not valid");
            }
        }

        return Ok(text.ToTextDto());
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllFromCurrentUser() {
        string userName = User.GetUserName();
        AppUser? user = await _userManager.FindByNameAsync(userName);
        if (user is null) {
            return StatusCode(500, "User does not exist");
        }

        var texts = await _textRepository.GetUsersTexts(userName);
        return Ok(texts.Select(t => t.ToTextWithoutContentDto()));
    }
}