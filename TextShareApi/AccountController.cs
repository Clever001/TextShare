using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.Dtos.Accounts;
using TextShareApi.Extensions;
using TextShareApi.Interfaces;
using TextShareApi.Mappers;
using TextShareApi.Models;

namespace TextShareApi;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase {
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager,
        ITokenService tokenManager,
        SignInManager<AppUser> signInManager) {
        _userManager = userManager;
        _tokenService = tokenManager;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto) {
        try {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            AppUser newUser = new() {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };
            
            var createdUser = await _userManager.CreateAsync(newUser, registerDto.Password);
            if (createdUser.Succeeded) {
                var roleResult = await _userManager.AddToRoleAsync(newUser, "User");
                if (roleResult.Succeeded) {
                    var userDto = newUser.ToNewUserDto(_tokenService.CreateToken(newUser));
                    return Ok(userDto);
                }
                
                return StatusCode(500, roleResult.Errors);
            }

            return StatusCode(500, createdUser.Errors);
        }
        catch (Exception ex) {
            return StatusCode(500, ex.ToExceptionDto());
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        AppUser? user = null;
        if (!string.IsNullOrEmpty(loginDto.UserName)) {
            user = await _userManager.FindByNameAsync(loginDto.UserName);
        } else if (!string.IsNullOrEmpty(loginDto.Email)) {
            user = await _userManager.FindByEmailAsync(loginDto.Email);
        }

        if (user is null) {
            return Unauthorized("Invalid username or password");
        }

        if (await _userManager.CheckPasswordAsync(user, loginDto.Password)) {
            return Ok(user.ToNewUserDto(_tokenService.CreateToken(user)));
        }
        
        return Unauthorized("Invalid username or password");
    }
}