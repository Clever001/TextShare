using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.Dtos.Accounts;
using TextShareApi.Extensions;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;

namespace TextShareApi.Controllers;

[Route("api/friendRequests")]
[ApiController]
public class FriendRequestController : ControllerBase {
    private IFriendRequestService _frService;
    public FriendRequestController(IFriendRequestService frService) {
        _frService = frService;
    }
    
    
    [HttpPost("requests/{recipientName}")]
    [Authorize]
    public async Task<IActionResult> CreateFriendRequest([FromRoute] string recipientName) {
        var curUser = User.GetUserName();
        
        var frResult = await _frService.Create(curUser, recipientName);
        if (!frResult.IsSuccess) {
            if (frResult.IsClientError) {
                return BadRequest(frResult.Error);
            }
            return StatusCode(500, frResult.Error);
        }

        return Ok(frResult.Value.ToDto());
    }

    [HttpDelete("requests/{recipientName}")]
    [Authorize]
    public async Task<IActionResult> DeleteFriendRequest([FromRoute] string recipientName) {
        var curUser = User.GetUserName();
        
        var deletionResult = await _frService.Delete(curUser, recipientName);
        if (!deletionResult.IsSuccess) {
            if (deletionResult.IsClientError) {
                return BadRequest(deletionResult.Error);
            }
            return StatusCode(500, deletionResult.Error);
        }
        
        return NoContent();
    }
    
    [HttpGet("requests/fromMe/")]
    [Authorize]
    public async Task<IActionResult> GetRequestsFromMe() {
        var curUser = User.GetUserName();

        var getResult = await _frService.GetSentFriendRequests(curUser);
        if (!getResult.IsSuccess) {
            if (getResult.IsSuccess) {
                return BadRequest(getResult.Error);
            }
            return StatusCode(500, getResult.Error);
        }
        
        return Ok(getResult.Value.Select(x => x.ToDto()).ToArray());
    }

    [HttpGet("requests/toMe/")]
    [Authorize]
    public async Task<IActionResult> GetRequestsToMe() {
        var curUser = User.GetUserName();

        var getResult = await _frService.GetReceivedFriendRequests(curUser);
        if (!getResult.IsSuccess) {
            if (getResult.IsSuccess) {
                return BadRequest(getResult.Error);
            }
            return StatusCode(500, getResult.Error);
        }
        
        return Ok(getResult.Value.Select(x => x.ToDto()).ToArray());
    }

    [HttpPut("requests/{senderName}")]
    [Authorize]
    public async Task<IActionResult> ProcessFriendRequest([FromRoute] string senderName, [FromBody] ProcessFriendRequestDto requestDto) {
        var curUser = User.GetUserName();

        var processionResult = await _frService.Process(senderName, curUser, requestDto.AcceptRequest);
        if (!processionResult.IsSuccess) {
            if (processionResult.IsSuccess) {
                return BadRequest(processionResult.Error);
            }
            return StatusCode(500, processionResult.Error);
        }
        
        return Ok(processionResult.Value.ToDto());
    }
}