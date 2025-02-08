using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.Dtos.Text;
using TextShareApi.Extensions;
using TextShareApi.Interfaces;
using TextShareApi.Mappers;
using TextShareApi.Models;
using TextShareApi.Services;

namespace TextShareApi.Controllers;

[Route("api/text")]
[ApiController]
public class TextController : ControllerBase {
    private readonly ITextRepository _textRepository;
    private readonly IUniqueIdService _uniqueIdService;
    private readonly UserManager<AppUser> _userManager;

    public TextController(ITextRepository textRepository,
        IUniqueIdService uniqueIdService,
        UserManager<AppUser> userManager) {
        _textRepository = textRepository;
        _uniqueIdService = uniqueIdService;
        _userManager = userManager;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateTextDto createTextDto) {
        string userName = User.GetUserName();
        AppUser? user = await _userManager.FindByNameAsync(userName);
        if (user is null) {
            return Unauthorized("User does not exist");
        }
        
        string newHash = await _uniqueIdService.GenerateNewHash();
        var text = new Text {
            Id = newHash,
            Content = createTextDto.Text ?? string.Empty,
            AppUserId = user.Id,
        };
        text = await _textRepository.CreateText(text);
        return Ok(text.ToTextDto(userName));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id) {
        var text = await _textRepository.GetTextById(id);
        if (text is null) return NotFound();

        return Ok(text.ToTextDto());
    }
}