using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.Dtos.Text;
using TextShareApi.Extensions;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;
using TextShareApi.Models;
using TextShareApi.Models.Enums;

namespace TextShareApi.Controllers;

[Route("api/texts")]
[ApiController]
public class TextController : ControllerBase {
    private readonly ITextRepository _textRepository;
    private readonly ITextSecuritySettingsRepository _securitySettingsRepository;
    private readonly IUniqueIdService _uniqueIdService;
    private readonly UserManager<AppUser> _userManager;
    private readonly PasswordHasher<AppUser> _passwordHasher;
    private readonly ITextSecurityService _textSecurityService;

    public TextController(ITextRepository textRepository,
        ITextSecuritySettingsRepository securitySettingsRepository,
        IUniqueIdService uniqueIdService,
        IFriendPairRepository friendPairRepository,
        UserManager<AppUser> userManager,
        PasswordHasher<AppUser> passwordHasher,
        ITextSecurityService textSecurityService) {
        _textRepository = textRepository;
        _securitySettingsRepository = securitySettingsRepository;
        _uniqueIdService = uniqueIdService;
        _userManager = userManager;
        _passwordHasher = passwordHasher;
        _textSecurityService = textSecurityService;
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
        text = await _textRepository.CreateText(text);

        AccessType accessType;
        if (createTextDto.Personal) accessType = AccessType.Personal;
        else if (createTextDto.ByReferencePublic) accessType = AccessType.ByReferencePublic;
        else if (createTextDto.ByReferenceAuthorized) accessType = AccessType.ByReferenceAuthorized;
        else accessType = AccessType.OnlyFriends;
        var settings = await _textSecurityService.ProvideTextSecuritySettings(text, accessType, createTextDto.Password);

        return Ok(text.ToTextDto(userName)); // TODO : Insert Security settings in dto.
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id, [FromQuery] string? requestPassword) {
        string senderName = User.GetUserName();
        AppUser? sender = await _userManager.FindByNameAsync(senderName);

        var response = await _textSecurityService.GetTextWithSecurityCheck(id, sender, requestPassword);

        return response.Details switch {
            SecurityCheckResult.Allowed => Ok(response.Text!.ToTextDto()),
            SecurityCheckResult.Forbidden => Forbid("User does not have access to this text"),
            SecurityCheckResult.NoPasswordProvided => Forbid("No password provided"),
            SecurityCheckResult.PasswordIsNotValid => Forbid("Password is not valid"),
            _ => StatusCode(500, "Internal Server Error")
        };
    }

    /// <summary>
    /// Данный метод возвращает все тексты, к которым имеется доступ у данного пользователя.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll() {
        // TODO: Вставить в запрос фильтрацию на тексты.
        throw new NotImplementedException();
    }

    [HttpPut("content/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateContent([FromRoute] string id) {
        throw new NotImplementedException();
    }

    [HttpPut("security/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateSecuritySettings([FromRoute] string id) {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] string id) {
        throw new NotImplementedException();
    }
}