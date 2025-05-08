using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.Dtos.Text;
using TextShareApi.Extensions;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;

namespace TextShareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TextController : ControllerBase {
    private readonly ILogger<TextController> _logger;
    private readonly ITextService _textService;

    public TextController(ITextService textService,
        ILogger<TextController> logger) {
        _textService = textService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateTextDto createTextDto) {
        var senderName = User.GetUserName();
        if (senderName == null) {
            _logger.LogCritical("Sender name is null.");
            throw new ArgumentNullException(nameof(senderName));
        }

        var result = await _textService.Create(senderName, createTextDto);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        return CreatedAtAction(nameof(GetById),
            new { id = result.Value.Id },
            result.Value.ToTextWithoutContentDto());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id, [FromQuery] string? requestPassword) {
        var senderName = User.GetUserName();
        
        var getResult = await _textService.GetById(id, senderName, requestPassword);
        if (!getResult.IsSuccess) return this.ToActionResult(getResult.Exception);
        
        return Ok(getResult.Value.ToTextDto());
    }

    [HttpGet("/myTexts")]
    [Authorize]
    public async Task<IActionResult> GetMyTexts() {
        var senderName = User.GetUserName();
        if (senderName == null) {
            _logger.LogCritical("Sender name is null.");
            throw new ArgumentNullException(nameof(senderName));
        }

        var result = await _textService.GetAccountTexts(senderName);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        
        return Ok(result.Value.Select(t => t.ToTextWithoutContentDto()).ToList());
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAvailable() {
        var senderName = User.GetUserName();

        var result = await _textService.GetAllAvailable(senderName);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        
        return Ok(result.Value.Select(t => t.ToTextWithoutContentDto()).ToList());
    }

    [HttpPut("{textId}")]
    [Authorize]
    public async Task<IActionResult> Update([FromRoute] string textId, [FromBody] UpdateTextDto updateTextDto,
        [FromQuery] string? requestPassword) {
        var senderName = User.GetUserName();
        if (senderName == null) {
            _logger.LogCritical("Sender name is null.");
            throw new ArgumentNullException(nameof(senderName));
        }

        var result = await _textService.Update(textId, senderName, requestPassword, updateTextDto);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        
        return Ok(result.Value.ToTextDto());
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] string id, [FromQuery] string? requestPassword) {
        var senderName = User.GetUserName();
        if (senderName == null) {
            _logger.LogCritical("Sender name is null.");
            throw new ArgumentNullException(nameof(senderName));
        }

        var result = await _textService.Delete(id, senderName, requestPassword);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        
        return NoContent();
    }
}