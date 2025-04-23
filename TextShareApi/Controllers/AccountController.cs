using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.Data;
using TextShareApi.Dtos.Accounts;
using TextShareApi.Dtos.Additional;
using TextShareApi.Extensions;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;
using TextShareApi.Models;

namespace TextShareApi.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase {
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(ITokenService tokenService, 
        UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager,
        AppDbContext context) {
        _tokenService = tokenService;
        _userManager = userManager;
        _signInManager = signInManager;
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
        // TODO: Добавить распределение по страницам
        var users = await _userManager.GetUsersInRoleAsync("User");
        return Ok(users.Select(u => u.ToUserWithoutTokenDto()));
    }
    
    private bool IsEmail(string input) {
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(input, emailPattern);
    }
}