using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Dtos.Accounts;
using TextShareApi.Extensions;
using TextShareApi.Models;

namespace TextShareApi.Controllers;

[Route("api/friends")]
[ApiController]
public class FriendController : ControllerBase {
    private UserManager<AppUser> _userManager;
    private AppDbContext _context;

    public FriendController(UserManager<AppUser> userManager, AppDbContext context) {
        _userManager = userManager;
        _context = context;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get() {
        throw new NotImplementedException();
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id) {
        throw new NotImplementedException();
    }
}