using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.Dtos.Accounts;
using TextShareApi.Dtos.QueryOptions;
using TextShareApi.Extensions;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;

namespace TextShareApi.Controllers;

[Route("api/friendRequests")]
[ApiController]
public class FriendRequestController : ControllerBase {
    private readonly IFriendRequestService _frService;
    private readonly ILogger<FriendRequestController> _logger;

    public FriendRequestController(IFriendRequestService frService,
        ILogger<FriendRequestController> logger) {
        _frService = frService;
        _logger = logger;
    }


    [HttpPost("{recipientName}")]
    [Authorize]
    public async Task<IActionResult> CreateFriendRequest([FromRoute] string recipientName) {
        var senderName = User.GetUserName();

        var result = await _frService.Create(senderName!, recipientName);

        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        return Ok(result.Value.ToDto());
    }

    [HttpDelete("{recipientName}")]
    [Authorize]
    public async Task<IActionResult> DeleteFriendRequest([FromRoute] string recipientName) {
        var senderName = User.GetUserName();

        var result = await _frService.Delete(senderName!, recipientName);

        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        return NoContent();
    }

    [HttpGet("fromMe/")]
    [Authorize]
    public async Task<IActionResult> GetRequestsFromMe([FromQuery] PaginationDto pagination,
        [FromQuery] bool isAscending, [FromQuery] string? recipientName) {
        var senderName = User.GetUserName();

        var result = await _frService.GetSentFriendRequests(
            pagination: pagination,
            isAscending: isAscending,
            senderName: senderName!,
            recipientName: recipientName
        );
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        return Ok(result.Value.Convert(r => r.ToDto()));
    }

    [HttpGet("toMe/")]
    [Authorize]
    public async Task<IActionResult> GetRequestsToMe([FromQuery] PaginationDto pagination,
        [FromQuery] bool isAscending, [FromQuery] string? senderName) {
        var recipientName = User.GetUserName();

        var result = await _frService.GetReceivedFriendRequests(
            pagination: pagination,
            isAscending: isAscending,
            senderName: senderName,
            recipientName: recipientName!
        );
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        return Ok(result.Value.Convert(r => r.ToDto()));
    }

    [HttpGet("fromMe/{recipientName}")]
    [Authorize]
    public async Task<IActionResult> GetRequestDetailFromMe([FromRoute] string recipientName)
    {
        var senderName = User.GetUserName();

        var result = await _frService.GetFriendRequest(senderName!, recipientName);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        return Ok(result.Value.ToDto());
    }

    [HttpGet("toMe/{senderName}")]
    [Authorize]
    public async Task<IActionResult> GetRequestDetailtoMe([FromRoute] string senderName)
    {
        var recipientName = User.GetUserName();

        var result = await _frService.GetFriendRequest(senderName, recipientName!);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        return Ok(result.Value.ToDto());
    }

    [HttpPut("{senderName}")]
    [Authorize]
    public async Task<IActionResult> ProcessFriendRequest([FromRoute] string senderName,
        [FromBody] ProcessFriendRequestDto requestDto) {
        var curUserName = User.GetUserName();

        var result = await _frService.Process(senderName, curUserName!, requestDto.AcceptRequest);

        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        
        return Ok(result.Value.ToDto());
    }
}