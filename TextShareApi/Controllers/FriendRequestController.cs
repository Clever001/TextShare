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
        Debug.Assert(senderName != null);

        var result = await _frService.Create(senderName, recipientName);
        
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        
        return Ok(result.Value.ToDto());
    }

    [HttpDelete("{recipientName}")]
    [Authorize]
    public async Task<IActionResult> DeleteFriendRequest([FromRoute] string recipientName) {
        var senderName = User.GetUserName();
        Debug.Assert(senderName != null);

        var result = await _frService.Delete(senderName, recipientName);
        
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        
        return NoContent();
    }

    [HttpGet("fromMe/")]
    [Authorize]
    public async Task<IActionResult> GetRequestsFromMe() {
        var senderName = User.GetUserName();
        Debug.Assert(senderName != null);

        var result = await _frService.GetSentFriendRequests(senderName);
        
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        
        return Ok(result.Value.Select(x => x.ToDto()).ToArray());
    }

    [HttpGet("toMe/")]
    [Authorize]
    public async Task<IActionResult> GetRequestsToMe() {
        var senderName = User.GetUserName();
        Debug.Assert(senderName != null);

        var result = await _frService.GetReceivedFriendRequests(senderName);
        
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        
        return Ok(result.Value.Select(x => x.ToDto()).ToArray());
    }

    [HttpPut("{senderName}")]
    [Authorize]
    public async Task<IActionResult> ProcessFriendRequest([FromRoute] string senderName,
        [FromBody] ProcessFriendRequestDto requestDto) {
        var curUserName = User.GetUserName();
        Debug.Assert(curUserName != null);

        var result = await _frService.Process(senderName, curUserName, requestDto.AcceptRequest);

        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        
        return Ok(result.Value.ToDto());
    }
}