using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.Dtos.Text;
using TextShareApi.Extensions;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;
using TextShareApi.Models.Enums;

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
    public async Task<IActionResult> Create() {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var senderName = User.GetUserName();
        if (senderName == null) {
            _logger.LogCritical("Sender name is null");
            throw new ArgumentNullException(nameof(senderName));
        }

        var result = await _textService.Create(senderName);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        return CreatedAtAction(nameof(GetById),
            new { id = result.Value.Id },
            result.Value.ToTextDto());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id, [FromQuery] string? requestPassword) {
        var senderName = User.GetUserName();
        
        var existenceCheck = await _textService.Contains(id);
        if (!existenceCheck.IsSuccess) return this.ToActionResult(existenceCheck.Exception);


        var getResult = await _textService.GetById(id, senderName, requestPassword);
        if (!getResult.IsSuccess) return this.ToActionResult(getResult.Exception);
        
        return Ok(getResult.Value.ToTextDto());
    }

    [HttpGet("/myTexts")]
    [Authorize]
    public async Task<IActionResult> GetMyTexts() {
        var senderName = User.GetUserName();
        if (senderName == null) {
            _logger.LogCritical("Sender name is null");
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

    [HttpPut("content/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateContent([FromRoute] string id,
        [FromQuery] string? requestPassword,
        [FromBody] TextContentUpdateDto updateDto) {
        if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);

        var senderName = User.GetUserName();
        if (senderName == null) {
            _logger.LogCritical("Sender name is null");
            throw new ArgumentNullException(nameof(senderName));
        }
        
        var existenceCheck = await _textService.Contains(id);
        if (!existenceCheck.IsSuccess) return this.ToActionResult(existenceCheck.Exception);

        var contentDto = new UpdateTextDto {
            Text = updateDto.Text,
            UpdatePassword = false
        };

        var updateResult = await _textService.Update(id, senderName, requestPassword, contentDto);
        if (!updateResult.IsSuccess) return this.ToActionResult(updateResult.Exception);
        
        return Ok(updateResult.Value.ToTextDto());
    }

    [HttpPut("security/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateSecuritySettings([FromRoute] string id,
        [FromQuery] string? requestPassword,
        [FromBody] TextSecSetUpdateDto secSetsDto) {
        if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);

        var senderName = User.GetUserName();
        if (senderName == null) {
            _logger.LogCritical("Sender name is null");
            throw new ArgumentNullException(nameof(senderName));
        }
        
        var existenceCheck = await _textService.Contains(id);
        if (!existenceCheck.IsSuccess) return this.ToActionResult(existenceCheck.Exception);

        var updateDto = new UpdateTextDto {
            Password = secSetsDto.Password,
            UpdatePassword = secSetsDto.UpdatePassword
        };

        if (secSetsDto.AccessType != null) {
            if (Enum.TryParse(secSetsDto.AccessType, out AccessType newType))
                updateDto.AccessType = newType;
            else
                return BadRequest("Invalid access type");
        }

        var updateResult = await _textService.Update(id, senderName, requestPassword, updateDto);
        if (!updateResult.IsSuccess) return this.ToActionResult(updateResult.Exception);
        
        return Ok(updateResult.Value.ToTextDto());
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] string id, [FromQuery] string? requestPassword) {
        var senderName = User.GetUserName();
        if (senderName == null) {
            _logger.LogCritical("Sender name is null");
            throw new ArgumentNullException(nameof(senderName));
        }

        var result = await _textService.Delete(id, senderName, requestPassword);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        
        return NoContent();
    }
}