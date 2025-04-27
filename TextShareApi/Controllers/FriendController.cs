using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.ClassesLib;
using TextShareApi.Extensions;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;

namespace TextShareApi.Controllers;

[Route("api/friends")]
[ApiController]
public class FriendController : ControllerBase {
    private readonly IFriendService _friendService;
    private readonly ILogger<FriendController> _logger;

    public FriendController(IFriendService friendService,
        ILogger<FriendController> logger) {
        _friendService = friendService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetFriends() {
        using var sectionTimer = SectionTimer.StartNew(_logger);

        var senderName = User.GetUserName();
        Debug.Assert(senderName != null);

        var result = await _friendService.GetFriends(senderName);
        if (!result.IsSuccess) return StatusCode(500, result.Error);

        sectionTimer.SetMessage($"Returned list of friends from {senderName}");

        return Ok(result.Value.Select(u => u.ToUserWithoutTokenDto()).ToArray());
    }

    [HttpGet("{userName}")]
    [Authorize]
    public async Task<IActionResult> AreFriends([FromRoute] string userName) {
        using var sectionTimer = SectionTimer.StartNew(_logger);

        var senderName = User.GetUserName();
        Debug.Assert(senderName != null);

        var result = await _friendService.AreFriends(senderName, userName);
        if (!result.IsSuccess) {
            if (result.IsClientError) return BadRequest(result.Error);
            return StatusCode(500, result.Error);
        }

        sectionTimer.SetMessage($"Answered are {senderName} and {userName} friends already.");

        return Ok(result.Value ? "Are friends" : "Are not friends");
    }

    [HttpDelete("{userName}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] string userName) {
        using var sectionTimer = SectionTimer.StartNew(_logger);

        var senderName = User.GetUserName();
        Debug.Assert(senderName != null);

        var result = await _friendService.RemoveFriend(senderName, userName);
        if (!result.IsSuccess) {
            if (result.IsClientError) return BadRequest(result.Error);
            return StatusCode(500, result.Error);
        }

        sectionTimer.SetMessage($"Deleted friendship between {senderName} and {userName}");

        return Ok("Friend deleted");
    }
}