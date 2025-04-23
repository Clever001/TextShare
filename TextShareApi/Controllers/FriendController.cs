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
        string curUser = User.GetUserName();

        var result = await _friendService.GetFriends(curUser);
        if (!result.IsSuccess) {
            return StatusCode(500, result.Error);
        }
        
        return Ok(result.Value.Select(u => u.ToUserWithoutTokenDto()).ToArray());
    }

    [HttpGet("{userName}")]
    [Authorize]
    public async Task<IActionResult> AreFriends([FromRoute] string userName) {
        string curUser = User.GetUserName();
        
        var result = await _friendService.AreFriends(curUser, userName);
        if (!result.IsSuccess) {
            if (result.IsClientError) {
                return BadRequest(result.Error);
            }
            return StatusCode(500, result.Error);
        }
        
        return Ok(result.Value ? "Are friends" : "Are not friends");
    }

    [HttpDelete("{userName}")]
    public async Task<IActionResult> Delete([FromRoute] string userName) {
        string curUser = User.GetUserName();
        
        var result = await _friendService.RemoveFriend(curUser, userName);
        if (!result.IsSuccess) {
            if (result.IsClientError) {
                return BadRequest(result.Error);
            }
            return StatusCode(500, result.Error);
        }
        
        return Ok("Friend deleted");
    }
}