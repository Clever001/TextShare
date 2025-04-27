using System.Diagnostics;
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
        var senderName = User.GetUserName();
        Debug.Assert(senderName != null);

        Debug.WriteLine("Started fr controller");
        var frResult = await _frService.Create(senderName, recipientName);
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
        var senderName = User.GetUserName();
        Debug.Assert(senderName != null);
        
        var deletionResult = await _frService.Delete(senderName, recipientName);
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
        var senderName = User.GetUserName();
        Debug.Assert(senderName != null);

        var getResult = await _frService.GetSentFriendRequests(senderName);
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
        var senderName = User.GetUserName();
        Debug.Assert(senderName != null);

        var getResult = await _frService.GetReceivedFriendRequests(senderName);
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
        var curUserName = User.GetUserName();
        Debug.Assert(curUserName != null);

        var processionResult = await _frService.Process(senderName, curUserName, requestDto.AcceptRequest);
        if (!processionResult.IsSuccess) {
            if (processionResult.IsClientError) {
                return BadRequest(processionResult.Error);
            }
            return StatusCode(500, processionResult.Error);
        }
        
        return Ok(processionResult.Value.ToDto());
    }
}