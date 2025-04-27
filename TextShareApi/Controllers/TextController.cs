using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.ClassesLib;
using TextShareApi.Dtos.Text;
using TextShareApi.Extensions;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;
using TextShareApi.Models;
using TextShareApi.Models.Enums;

namespace TextShareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TextController : ControllerBase {
    private readonly ITextService _textService; 

    public TextController(ITextService textService) {
        _textService = textService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create() {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        string? senderName = User.GetUserName(); // TODO: Fix to return nullable type.
        Debug.Assert(senderName != null);

        Result<Text> creationResult = await _textService.Create(senderName);
        if (!creationResult.IsSuccess) {
            if (creationResult.IsClientError) {
                return BadRequest(creationResult.Error);
            }
            return StatusCode(500, creationResult.Error);
        }

        return CreatedAtAction(nameof(GetById),
            new { id = creationResult.Value.Id },
            creationResult.Value.ToTextDto());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id, [FromQuery] string? requestPassword) {
        string? senderName = User.GetUserName();

        for (int i = 0; i != 10; i++) {
            Debug.WriteLine("From Debug!");
        }
        
        Result<Text> getResult = await _textService.GetById(id, senderName, requestPassword);
        if (!getResult.IsSuccess) {
            if (getResult.IsClientError) {
                return Forbid();
            }
            return StatusCode(500, getResult.Error);
        }
        
        return Ok(getResult.Value.ToTextDto());
    }

    [HttpGet("/myTexts")]
    [Authorize]
    public async Task<IActionResult> GetMyTexts() {
        string? senderName = User.GetUserName();
        Debug.Assert(senderName != null);
        
        Result<List<Text>> getResult = await _textService.GetAccountTexts(senderName);
        if (!getResult.IsSuccess) {
            if (getResult.IsClientError) {
                return Forbid();
            }
            return StatusCode(500, getResult.Error);
        }
        
        return Ok(getResult.Value.Select(t => t.ToTextWithoutContentDto()).ToList());
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAvailable() {
        string? senderName = User.GetUserName();
        
        Result<List<Text>> getResult = await _textService.GetAllAvailable(senderName);
        if (!getResult.IsSuccess) {
            if (getResult.IsClientError) {
                return BadRequest(getResult.Error);
            }
            return StatusCode(500, getResult.Error);
        }
        
        return Ok(getResult.Value.Select(t => t.ToTextWithoutContentDto()).ToList());
    }

    [HttpPut("content/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateContent([FromRoute] string id,
        [FromQuery] string? requestPassword,
        [FromBody] TextContentUpdateDto updateDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState.ValidationState);
        }
        
        string? senderName = User.GetUserName();
        Debug.Assert(senderName != null);

        var contentDto = new UpdateTextDto {
            Text = updateDto.Text,
            UpdatePassword = false,
        };

        var updateResult = await _textService.Update(id, senderName, requestPassword, contentDto);
        if (!updateResult.IsSuccess) {
            if (updateResult.IsClientError) {
                return Forbid();
            }
            return StatusCode(500, updateResult.Error);
        }
        
        return Ok(updateResult.Value.ToTextDto());
    }

    [HttpPut("security/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateSecuritySettings([FromRoute] string id,
        [FromQuery] string? requestPassword,
        [FromBody] TextSecSetUpdateDto secSetsDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState.ValidationState);
        }

        string? senderName = User.GetUserName();
        Debug.Assert(senderName != null);
        
        var updateDto = new UpdateTextDto {
            Password = secSetsDto.Password,
            UpdatePassword = secSetsDto.UpdatePassword,
        };

        if (secSetsDto.AccessType != null) {
            if (Enum.TryParse(secSetsDto.AccessType, out AccessType newType)) {
                updateDto.AccessType = newType;
            }
            else {
                return BadRequest("Invalid access type");
            }
        }
        
        Result<Text> updateResult = await _textService.Update(id, senderName, requestPassword, updateDto);
        if (!updateResult.IsSuccess) {
            if (updateResult.IsClientError) {
                return Forbid();
            }
            return StatusCode(500, updateResult.Error);
        }

        return Ok(updateResult.Value.ToTextDto());
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] string id, [FromQuery] string? requestPassword) {
        string? senderName = User.GetUserName();
        Debug.Assert(senderName != null);

        Result deleteResult = await _textService.Delete(id, senderName, requestPassword);
        if (!deleteResult.IsSuccess) {
            if (deleteResult.IsClientError) {
                return Forbid();
            }
            return StatusCode(500, deleteResult.Error);
        }

        return NoContent();
    }
}