using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Dtos.Accounts;
using TextShareApi.Extensions;
using TextShareApi.Interfaces;
using TextShareApi.Mappers;
using TextShareApi.Models;

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
            return BadRequest(frResult.Error);
        }

        return Ok(frResult.Value.ToDto());
    }

    [HttpDelete("requests/{recipientName}")]
    [Authorize]
    public async Task<IActionResult> DeleteFriendRequest([FromRoute] string recipientName) {
        var curUser = User.GetUserName();
        
        var deletionResult = await _frService.Delete(curUser, recipientName);
        if (!deletionResult.IsSuccess) {
            return BadRequest(deletionResult.Error);
        }
        
        return NoContent();
    }
    
    [HttpGet("requests/fromMe/")]
    [Authorize]
    public async Task<IActionResult> GetRequestsFromMe() {
        var curUser = User.GetUserName();

        var getResult = await _frService.GetSentFriendRequests(curUser);
        if (!getResult.IsSuccess) {
            return BadRequest(getResult.Error);
        }
        
        return Ok(getResult.Value.Select(x => x.ToDto()));
    }

    [HttpGet("requests/toMe/")]
    [Authorize]
    public async Task<IActionResult> GetRequestsToMe() {
        var curUser = User.GetUserName();

        var getResult = await _frService.GetReceivedFriendRequests(curUser);
        if (!getResult.IsSuccess) {
            return BadRequest(getResult.Error);
        }
        
        return Ok(getResult.Value.Select(x => x.ToDto()));
    }

    [HttpPut("requests/{senderName}")]
    [Authorize]
    public async Task<IActionResult> ProcessFriendRequest([FromRoute] string senderName, [FromBody] ProcessFriendRequestDto requestDto) {
        var curUser = User.GetUserName();

        var processionResult = await _frService.Process(senderName, curUser, requestDto.AcceptRequest);
        if (!processionResult.IsSuccess) {
            return BadRequest(processionResult.Error);
        }
        
        return Ok(processionResult.Value.ToDto());
    }
}