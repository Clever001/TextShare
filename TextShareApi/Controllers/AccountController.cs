using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.Dtos.Accounts;
using TextShareApi.Dtos.Additional;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;

namespace TextShareApi.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase {
    private readonly IAccountService _accountService;
    private readonly IAccountRepository _accountRepository;

    public AccountController(IAccountService accountService,
        IAccountRepository accountRepository) {
        _accountService = accountService;
        _accountRepository = accountRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        if (IsEmail(registerDto.UserName)) {
            return BadRequest(new ExceptionDto {
                Code = "InvalidUserName",
                Description = "UserName cannot represent email address"
            });
        }
        
        var result = await _accountService.Register(
            registerDto.UserName, registerDto.Email, registerDto.Password);

        if (!result.IsSuccess) {
            if (result.IsClientError) {
                return BadRequest(result.Error);
            }
            return StatusCode(500, result.Error);
        }

        var (user, token) = result.Value;

        return Ok(new UserWithTokenDto {
            UserName = user.UserName,
            Email = user.Email,
            Token = token
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        var result = await _accountService.Login(loginDto.UserNameOrEmail, loginDto.Password);

        if (!result.IsSuccess) {
            return Unauthorized(new ExceptionDto {
                Code = "ExceptionDuringLogin",
                Description = "Check your registration details for correctness"
            });
        }

        var (user, token) = result.Value;
        
        return Ok(new UserWithTokenDto {
            UserName = user.UserName,
            Email = user.Email,
            Token = token
        });
    }

    [HttpGet]
    public async Task<IActionResult> Get() {
        // TODO: Добавить распределение по страницам
        var users = await _accountRepository.GetUsers();
        return Ok(users.Select(u => u.ToUserWithoutTokenDto()).ToArray());
    }
    
    private bool IsEmail(string input) {
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(input, emailPattern);
    }
}