using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Dtos.Accounts;
using TextShareApi.Extensions;
using TextShareApi.Models;

namespace TextShareApi.Controllers;

[Route("api/friendRequests")]
[ApiController]
public class FriendRequestController : ControllerBase {
    private UserManager<AppUser> _userManager;
    private AppDbContext _context;

    public FriendRequestController(UserManager<AppUser> userManager, AppDbContext context) {
        _userManager = userManager;
        _context = context;
    }
    
    
    [HttpPost("requests/{userName}")]
    [Authorize]
    public async Task<IActionResult> CreateFriendRequest([FromRoute] string userName) {
        throw new NotImplementedException();
        var sender = await _userManager.FindByNameAsync(User.GetUserName());
        if (sender is null) {
            return StatusCode(500, "Current user not found");
        }
        var recipient = await _userManager.FindByNameAsync(userName);
        if (recipient is null) {
            return NotFound("Recipient user not found");
        }

        if (sender.Id == recipient.Id) {
            return BadRequest("Recipient and sender users cannot be the same");
        }
        
        if (await _context.FriendPairs.AnyAsync(p => p.FirstUserId == sender.Id && p.SecondUserId == recipient.Id)) {
            return BadRequest("Users are already friends");
        }
        
        if (await _context.FriendRequests
                .AnyAsync(r => r.SenderId == sender.Id && r.RecipientId == recipient.Id)) {
            return BadRequest("Friend request already exists");
        }

        if (await _context.FriendRequests
                .AnyAsync(r => r.SenderId == recipient.Id && r.RecipientId == sender.Id)) {
            return BadRequest("Cannot create request. Recipient user already sent request to you");
        }

        var request = new FriendRequest {
            SenderId = sender.Id,
            RecipientId = recipient.Id,
        };
        await _context.FriendRequests.AddAsync(request);
        await _context.SaveChangesAsync();
        return Ok("Friend request created");
    }

    [HttpDelete("requests/{userName}")]
    [Authorize]
    public async Task<IActionResult> DeleteFriendRequest([FromRoute] string userName) {
        throw new NotImplementedException();
        
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        var curUser = await _userManager.FindByNameAsync(User.GetUserName());
        if (curUser is null) {
            return StatusCode(500, "Current user not found");
        }

        var recipient = await _userManager.FindByNameAsync(userName);
        if (recipient is null) {
            return NotFound("Recipient user not found");
        }

        var request = await _context.FriendRequests.
            SingleOrDefaultAsync(r => r.SenderId == curUser.Id && r.RecipientId == recipient.Id);
        if (request is null) {
            return NotFound("Request to this user not found");
        }
        
        await _context.FriendPairs
            .Where(p => p.FirstUserId == recipient.Id && p.SecondUserId == curUser.Id ||
                        p.FirstUserId == curUser.Id && p.SecondUserId == recipient.Id)
            .ExecuteDeleteAsync();

        _context.FriendRequests.Remove(request);
            
        await _context.SaveChangesAsync();
        return Ok("Friend request deleted");
    }
    
    [HttpGet("requests/{userName}")]
    [Authorize]
    public async Task<IActionResult> GetFriendRequests([FromRoute] string userName) {
        // TODO: Добавить фильтрацию на запросы отправленные и полученные.
        throw new NotImplementedException();
        
        var curUser = await _userManager.FindByNameAsync(User.GetUserName());
        if (curUser is null) {
            return StatusCode(500, "Current user not found");
        }
        
        var sendersNames = await _context.FriendRequests
            .Include(r => r.Sender)
            .Where(r => r.RecipientId == curUser.Id)
            .ToListAsync();

        return Ok(sendersNames.Select(r => new {
            UserName = r.Sender.UserName,
            RequestStatus = r.IsAccepted switch {
                true => "Accepted",
                false => "Rejected",
                _ => "Active Request"
            }
        }).ToList());
    }

    [HttpPut("requests/{userName}")]
    [Authorize]
    public async Task<IActionResult> ProcessFriendRequest([FromRoute] string userName, [FromBody] ProcessFriendRequestDto requestDto) {
        // TODO: Delete username from dto.

        throw new NotImplementedException();
        
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        var curUser = await _userManager.FindByNameAsync(User.GetUserName());
        if (curUser is null) {
            return StatusCode(500, "Current user not found");
        }

        var sender = await _userManager.FindByNameAsync(requestDto.UserName);
        if (sender is null) {
            return NotFound("Sender user not found");
        }

        var request = await _context.FriendRequests.
            SingleOrDefaultAsync(r => r.SenderId == sender.Id && r.RecipientId == curUser.Id);
        if (request is null) {
            return NotFound("Request from this user not found");
        }

        if (requestDto.AcceptRequest) {
            List<FriendPair> friendPairs = [
                new() {
                    FirstUserId = curUser.Id,
                    SecondUserId = sender.Id,
                },
                new() {
                    FirstUserId = sender.Id,
                    SecondUserId = curUser.Id
                }
            ];

            await _context.FriendPairs.AddRangeAsync(friendPairs);
        }
        else {
            await _context.FriendPairs
                .Where(p => p.FirstUserId == sender.Id && p.SecondUserId == curUser.Id ||
                            p.FirstUserId == curUser.Id && p.SecondUserId == sender.Id)
                .ExecuteDeleteAsync();
        }

        request.IsAccepted = requestDto.AcceptRequest;
            
        await _context.SaveChangesAsync();
        return Ok(requestDto.AcceptRequest ? "Friend request accepted" : "Friend request rejected");
    }
}