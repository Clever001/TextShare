using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.Dtos.QueryOptions;
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
    public async Task<IActionResult> GetFriends([FromQuery] PaginationDto pagination, [FromQuery] bool isAscending, [FromQuery] string? friendName) {
        var senderName = User.GetUserName();

        var result = await _friendService.GetFriends(pagination, isAscending, friendName, senderName!);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        
        return Ok(result.Value.Select(u => u.ToUserWithoutTokenDto()).ToArray());
    }

    [HttpGet("areFriends/{userName}")]
    [Authorize]
    public async Task<IActionResult> AreFriends([FromRoute] string userName) {
        var senderName = User.GetUserName();
        
        var result = await _friendService.AreFriendsByName(senderName!, userName);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        
        return Ok(result.Value ? "Are friends" : "Are not friends");
    }

    [HttpDelete("{userName}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] string userName) {
        var senderName = User.GetUserName();

        var result = await _friendService.RemoveFriend(senderName!, userName);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        
        return Ok("Friend deleted");
    }
}