using Microsoft.AspNetCore.Authorization;
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
    private AppDbContext _context;

    public FriendRequestController(IFriendRequestService frService, AppDbContext context) {
        _frService = frService;
        _context = context;
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
    
    [HttpGet("requests/{userName}")]
    [Authorize]
    public async Task<IActionResult> GetFriendRequests([FromRoute] string userName) {
        // TODO: Implement this!
        throw new NotImplementedException();
    }

    [HttpPut("requests/{senderName}")]
    [Authorize]
    public async Task<IActionResult> ProcessFriendRequest([FromRoute] string senderName, [FromBody] ProcessFriendRequestDto requestDto) {
        // TODO: Check request dto.
        var curUser = User.GetUserName();

        var processionResult = await _frService.Process(senderName, curUser, requestDto.AcceptRequest);
        if (!processionResult.IsSuccess) {
            return BadRequest(processionResult.Error);
        }
        
        return Ok(processionResult.Value.ToDto());
    }
}