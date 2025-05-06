using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.ClassesLib;
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

        var creationResult = await _textService.Create(senderName);
        if (!creationResult.IsSuccess) {
            if (creationResult.IsClientError) return BadRequest(creationResult.Error);
            return StatusCode(500, creationResult.Error);
        }
        return CreatedAtAction(nameof(GetById),
            new { id = creationResult.Value.Id },
            creationResult.Value.ToTextDto());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id, [FromQuery] string? requestPassword) {
        var senderName = User.GetUserName();
        
        var existenceCheck = await _textService.Contains(id);
        if (!existenceCheck.IsSuccess) {
            return NotFound(existenceCheck.Error);
        }

        var getResult = await _textService.GetById(id, senderName, requestPassword);
        if (!getResult.IsSuccess) {
            if (getResult.IsClientError) return Forbid();
            return StatusCode(500, getResult.Error);
        }
        
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

        var getResult = await _textService.GetAccountTexts(senderName);
        if (!getResult.IsSuccess) {
            if (getResult.IsClientError) return Forbid();
            return StatusCode(500, getResult.Error);
        }
        
        return Ok(getResult.Value.Select(t => t.ToTextWithoutContentDto()).ToList());
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAvailable() {
        var senderName = User.GetUserName();

        var getResult = await _textService.GetAllAvailable(senderName);
        if (!getResult.IsSuccess) {
            if (getResult.IsClientError) return BadRequest(getResult.Error);
            return StatusCode(500, getResult.Error);
        }
        
        return Ok(getResult.Value.Select(t => t.ToTextWithoutContentDto()).ToList());
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
        if (!existenceCheck.IsSuccess) {
            return NotFound(existenceCheck.Error);
        }

        var contentDto = new UpdateTextDto {
            Text = updateDto.Text,
            UpdatePassword = false
        };

        var updateResult = await _textService.Update(id, senderName, requestPassword, contentDto);
        if (!updateResult.IsSuccess) {
            if (updateResult.IsClientError) return Forbid();
            return StatusCode(500, updateResult.Error);
        }
        
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
        if (!existenceCheck.IsSuccess) {
            return NotFound(existenceCheck.Error);
        }

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
        if (!updateResult.IsSuccess) {
            if (updateResult.IsClientError) return Forbid();
            return StatusCode(500, updateResult.Error);
        }
        
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

        var deleteResult = await _textService.Delete(id, senderName, requestPassword);
        if (!deleteResult.IsSuccess) {
            if (deleteResult.IsClientError) return Forbid();
            return StatusCode(500, deleteResult.Error);
        }
        
        return NoContent();
    }
}