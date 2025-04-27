using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.ClassesLib;
using TextShareApi.Dtos.Accounts;
using TextShareApi.Dtos.Additional;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;

namespace TextShareApi.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase {
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAccountService accountService,
        IAccountRepository accountRepository,
        ILogger<AccountController> logger) {
        _accountService = accountService;
        _accountRepository = accountRepository;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto) {
        using var sectionTimer = SectionTimer.StartNew(_logger);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (IsEmail(registerDto.UserName))
            return BadRequest(new ExceptionDto {
                Code = "InvalidUserName",
                Description = "UserName cannot represent email address"
            });

        var result = await _accountService.Register(
            registerDto.UserName, registerDto.Email, registerDto.Password);

        if (!result.IsSuccess) {
            if (result.IsClientError) return BadRequest(result.Error);
            return StatusCode(500, result.Error);
        }

        var (user, token) = result.Value;

        sectionTimer.SetMessage($"Registered new user: {user.UserName}");

        return Ok(new UserWithTokenDto {
            UserName = user.UserName,
            Email = user.Email,
            Token = token
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
        using var sectionTimer = SectionTimer.StartNew(_logger);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _accountService.Login(loginDto.UserNameOrEmail, loginDto.Password);

        if (!result.IsSuccess)
            return Unauthorized(new ExceptionDto {
                Code = "ExceptionDuringLogin",
                Description = "Check your registration details for correctness"
            });

        var (user, token) = result.Value;

        sectionTimer.SetMessage($"Logined user: {user.UserName}");

        return Ok(new UserWithTokenDto {
            UserName = user.UserName,
            Email = user.Email,
            Token = token
        });
    }

    [HttpGet]
    public async Task<IActionResult> Get() {
        using var sectionTimer = SectionTimer.StartNew(_logger);

        // TODO: Добавить распределение по страницам
        var users = await _accountRepository.GetUsers();

        sectionTimer.SetMessage($"Returned users: {users.Count}");

        return Ok(users.Select(u => u.ToUserWithoutTokenDto()).ToArray());
    }

    private bool IsEmail(string input) {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(input, emailPattern);
    }
}