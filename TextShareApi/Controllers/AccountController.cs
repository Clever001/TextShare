using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Dtos.Accounts;
using TextShareApi.Dtos.Additional;
using TextShareApi.Extensions;
using TextShareApi.Interfaces;
using TextShareApi.Mappers;
using TextShareApi.Models;

namespace TextShareApi.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase {
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly AppDbContext _context;

    public AccountController(ITokenService tokenService, 
        UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager,
        AppDbContext context) {
        _tokenService = tokenService;
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto) {
        try {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (IsEmail(registerDto.UserName)) {
                return BadRequest(new ExceptionDto {
                    Code = "InvalidUserName",
                    Description = "UserName cannot represent email address"
                });
            }

            var user = new AppUser {
                UserName = registerDto.UserName,
                Email = registerDto.Email
            };
            
            var createResult = await _userManager.CreateAsync(user, registerDto.Password);
            if (createResult.Succeeded) {
                var appendResult = await _userManager.AddToRoleAsync(user, "User");
                if (appendResult.Succeeded) {
                    string token = _tokenService.CreateToken(user);
                    return Ok(new UserWithTokenDto {
                        UserName = user.UserName,
                        Email = user.Email,
                        Token = token
                    });
                }
                
                return StatusCode(500, appendResult.Errors);
            }
            return StatusCode(500, createResult.Errors);
        }
        catch (Exception e) {
            return StatusCode(500, e.ToExceptionDto());
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
        IActionResult Unauth() {
            return Unauthorized(new ExceptionDto {
                Code = "ExceptionDuringLogin",
                Description = "Check your registration details for correctness"
            });
        }
        
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        AppUser? user = await _userManager.FindByNameAsync(loginDto.UserNameOrEmail);
        if (user is null) {
            user = await _userManager.FindByEmailAsync(loginDto.UserNameOrEmail);
        }

        if (user is null) {
            return Unauth();
        }
        
        bool passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (passwordValid) {
            string token = _tokenService.CreateToken(user);
            return Ok(new UserWithTokenDto {
                UserName = user.UserName,
                Email = user.Email,
                Token = token
            });
        }
        
        return Unauth();
    }

    [HttpGet]
    public async Task<IActionResult> Get() {
        var users = await _userManager.GetUsersInRoleAsync("User");
        return Ok(users.Select(u => u.ToUserWithoutTokenDto()));
    }
    
    // Edit endpoints to control friends list.
    // Insert DTOs proper way.
    // Replace _context with Repository class.

    [HttpGet("friend_requests/to_me")]
    [Authorize]
    public async Task<IActionResult> GetMyFriendRequestsToMe() {
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

    [HttpGet("friend_requests/from_me")]
    [Authorize]
    public async Task<IActionResult> GetMyFriendRequestsFromMe() {
        var curUser = await _userManager.FindByNameAsync(User.GetUserName());
        if (curUser is null) {
            return StatusCode(500, "Current user not found");
        }
        
        var sendersNames = await _context.FriendRequests
            .Include(r => r.Recipient)
            .Where(r => r.SenderId == curUser.Id)
            .ToListAsync();

        return Ok(sendersNames.Select(r => new {
            UserName = r.Recipient.UserName,
            RequestStatus = r.IsAccepted switch {
                true => "Accepted",
                false => "Rejected",
                _ => "Active Request"
            }
        }).ToList());
    }

    [HttpPost("friend_requests/create_request/{userName}")]
    [Authorize]
    public async Task<IActionResult> CreateFriendRequest([FromRoute] string userName) {
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

    [HttpPost("friend_requests/process_request/to_me")]
    [Authorize]
    public async Task<IActionResult> ProcessFriendRequestToMe([FromBody] ProcessFriendRequestToMeDto requestToMeDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        var curUser = await _userManager.FindByNameAsync(User.GetUserName());
        if (curUser is null) {
            return StatusCode(500, "Current user not found");
        }

        var sender = await _userManager.FindByNameAsync(requestToMeDto.UserName);
        if (sender is null) {
            return NotFound("Sender user not found");
        }

        var request = await _context.FriendRequests.
            SingleOrDefaultAsync(r => r.SenderId == sender.Id && r.RecipientId == curUser.Id);
        if (request is null) {
            return NotFound("Request from this user not found");
        }

        if (requestToMeDto.AcceptRequest) {
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

        /* // Commented. Now we cannot have two requests from both sides.
        var reverseRequest = await _context.FriendRequests
            .SingleOrDefaultAsync(r => r.SenderId == curUser.Id && r.RecipientId == sender.Id);
        if (reverseRequest is not null) {
            reverseRequest.IsAccepted = requestDto.AcceptRequest;
        }*/

        request.IsAccepted = requestToMeDto.AcceptRequest;
            
        await _context.SaveChangesAsync();
        return Ok(requestToMeDto.AcceptRequest ? "Friend request accepted" : "Friend request rejected");
    }

    [HttpDelete("friend_requests/process_request/from_me/{recipientName}")]
    [Authorize]
    public async Task<IActionResult> ProcessFriendRequestFromMe([FromRoute] string recipientName) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        var curUser = await _userManager.FindByNameAsync(User.GetUserName());
        if (curUser is null) {
            return StatusCode(500, "Current user not found");
        }

        var recipient = await _userManager.FindByNameAsync(recipientName);
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

    [HttpGet("friendsList")]
    [Authorize]
    public async Task<IActionResult> GetFriendsList() {
        var userName = User.GetUserName();
        var curUser = await _userManager.FindByNameAsync(userName);
        if (curUser is null) {
            return StatusCode(500, "Current user not found");
        }
        var friends = await _context.FriendPairs
            .Include(p => p.SecondUser)
            .Where(p => p.FirstUserId == curUser.Id)
            .Select(p => p.SecondUser.UserName)
            .ToListAsync();

        return Ok(friends.Select(n => new UserWithoutTokenDto {
            UserName = n,
        }).ToList());
    }
    
    private bool IsEmail(string input) {
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(input, emailPattern);
    }
}