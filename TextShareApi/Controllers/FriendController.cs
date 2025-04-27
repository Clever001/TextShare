using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.Extensions;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;

namespace TextShareApi.Controllers;

[Route("api/friends")]
[ApiController]
public class FriendController : ControllerBase {
    private readonly IFriendService _friendService;

    public FriendController(IFriendService friendService) {
        _friendService = friendService;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetFriends() {
        string? senderName = User.GetUserName();
        Debug.Assert(senderName != null);

        var result = await _friendService.GetFriends(senderName);
        if (!result.IsSuccess) {
            return StatusCode(500, result.Error);
        }
        
        return Ok(result.Value.Select(u => u.ToUserWithoutTokenDto()).ToArray());
    }

    [HttpGet("{userName}")]
    [Authorize]
    public async Task<IActionResult> AreFriends([FromRoute] string userName) {
        string? senderName = User.GetUserName();
        Debug.Assert(senderName != null);
        
        var result = await _friendService.AreFriends(senderName, userName);
        if (!result.IsSuccess) {
            if (result.IsClientError) {
                return BadRequest(result.Error);
            }
            return StatusCode(500, result.Error);
        }
        
        return Ok(result.Value ? "Are friends" : "Are not friends");
    }

    [HttpDelete("{userName}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] string userName) {
        string? senderName = User.GetUserName();
        Debug.Assert(senderName != null);
        
        var result = await _friendService.RemoveFriend(senderName, userName);
        if (!result.IsSuccess) {
            if (result.IsClientError) {
                return BadRequest(result.Error);
            }
            return StatusCode(500, result.Error);
        }
        
        return Ok("Friend deleted");
    }
}