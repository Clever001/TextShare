using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.ClassesLib;
using TextShareApi.Dtos.Accounts;
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
        using var sectionTimer = SectionTimer.StartNew(_logger);

        var senderName = User.GetUserName();
        Debug.Assert(senderName != null);

        Debug.WriteLine("Started fr controller");
        var frResult = await _frService.Create(senderName, recipientName);
        if (!frResult.IsSuccess) {
            if (frResult.IsClientError) return BadRequest(frResult.Error);
            return StatusCode(500, frResult.Error);
        }

        sectionTimer.SetMessage($"Created friend request between {senderName} and {recipientName}");

        return Ok(frResult.Value.ToDto());
    }

    [HttpDelete("{recipientName}")]
    [Authorize]
    public async Task<IActionResult> DeleteFriendRequest([FromRoute] string recipientName) {
        using var sectionTimer = SectionTimer.StartNew(_logger);

        var senderName = User.GetUserName();
        Debug.Assert(senderName != null);

        var deletionResult = await _frService.Delete(senderName, recipientName);
        if (!deletionResult.IsSuccess) {
            if (deletionResult.IsClientError) return BadRequest(deletionResult.Error);
            return StatusCode(500, deletionResult.Error);
        }

        sectionTimer.SetMessage($"Deleted friend request between {senderName} and {recipientName}");

        return NoContent();
    }

    [HttpGet("fromMe/")]
    [Authorize]
    public async Task<IActionResult> GetRequestsFromMe() {
        using var sectionTimer = SectionTimer.StartNew(_logger);

        var senderName = User.GetUserName();
        Debug.Assert(senderName != null);

        var getResult = await _frService.GetSentFriendRequests(senderName);
        if (!getResult.IsSuccess) {
            if (getResult.IsSuccess) return BadRequest(getResult.Error);
            return StatusCode(500, getResult.Error);
        }

        sectionTimer.SetMessage($"Returned friend requests from {senderName}");

        return Ok(getResult.Value.Select(x => x.ToDto()).ToArray());
    }

    [HttpGet("toMe/")]
    [Authorize]
    public async Task<IActionResult> GetRequestsToMe() {
        using var sectionTimer = SectionTimer.StartNew(_logger);

        var senderName = User.GetUserName();
        Debug.Assert(senderName != null);

        var getResult = await _frService.GetReceivedFriendRequests(senderName);
        if (!getResult.IsSuccess) {
            if (getResult.IsSuccess) return BadRequest(getResult.Error);
            return StatusCode(500, getResult.Error);
        }

        sectionTimer.SetMessage($"Returned friend requests to {senderName}");

        return Ok(getResult.Value.Select(x => x.ToDto()).ToArray());
    }

    [HttpPut("{senderName}")]
    [Authorize]
    public async Task<IActionResult> ProcessFriendRequest([FromRoute] string senderName,
        [FromBody] ProcessFriendRequestDto requestDto) {
        using var sectionTimer = SectionTimer.StartNew(_logger);

        var curUserName = User.GetUserName();
        Debug.Assert(curUserName != null);

        var processionResult = await _frService.Process(senderName, curUserName, requestDto.AcceptRequest);
        if (!processionResult.IsSuccess) {
            if (processionResult.IsClientError) return BadRequest(processionResult.Error);
            return StatusCode(500, processionResult.Error);
        }

        sectionTimer.SetMessage($"Processed friend request from {senderName}");

        return Ok(processionResult.Value.ToDto());
    }
}