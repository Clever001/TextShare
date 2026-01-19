using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.Dtos.QueryOptions;
using TextShareApi.Dtos.QueryOptions.Filters;
using TextShareApi.Dtos.Text;
using Shared.Exceptions;
using TextShareApi.Extensions;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;
using TextShareApi.Models;

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

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationDto pagination,
        [FromQuery] SortDto sort,
        [FromQuery] TextFilterDto filter) {
        if (!sort.IsValid(typeof(Text)))
            return this.ToActionResult(new BadRequestException("SortBy contains invalid property name."));

        var senderName = User.GetUserName();

        if (Request.Headers.ContainsKey("Authorization") && senderName is null or "")
            return this.ToActionResult(new ForbiddenException());

        var result = await _textService.GetTexts(pagination, sort, filter, senderName);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        return Ok(result.Value.Convert(t => t.ToTextWithoutContentDto()));
    }

    [HttpGet("byName")]
    public async Task<IActionResult> GetByUserName([FromQuery] PaginationDto pagination,
        [FromQuery] SortDto sort,
        [FromQuery] TextFilterWithoutOwnerDto filter, [FromQuery] string ownerName) {
        if (!sort.IsValid(typeof(Text)))
            return this.ToActionResult(new BadRequestException("SortBy contains invalid property name."));

        var senderName = User.GetUserName();

        if (Request.Headers.ContainsKey("Authorization") && senderName is null or "")
            return this.ToActionResult(new ForbiddenException());

        var result = await _textService.GetTextsByName(pagination, sort, filter, ownerName, senderName);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        return Ok(result.Value.Convert(t => t.ToTextWithoutContentDto()));
    }

    [HttpGet("latests")]
    public async Task<IActionResult> GetLatests() {
        var result = await _textService.GetLatestTexts();
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        return Ok(result.Value.Select(x => x.ToTextWithoutContentDto()).ToList());
    }

    [HttpPut("{textId}")]
    [Authorize]
    public async Task<IActionResult> Update([FromRoute] string textId, [FromBody] UpdateTextDto updateTextDto) {
        var senderName = User.GetUserName();
        if (senderName == null) {
            _logger.LogCritical("Sender name is null.");
            throw new ArgumentNullException(nameof(senderName));
        }

        var result = await _textService.Update(textId, senderName, updateTextDto);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        return Ok(result.Value.ToTextDto());
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] string id) {
        var senderName = User.GetUserName();
        if (senderName == null) {
            _logger.LogCritical("Sender name is null.");
            throw new ArgumentNullException(nameof(senderName));
        }

        var result = await _textService.Delete(id, senderName);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        return NoContent();
    }
}