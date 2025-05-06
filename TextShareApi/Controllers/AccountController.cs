using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.Attributes;
using TextShareApi.Dtos.Accounts;
using TextShareApi.Dtos.Exception;
using TextShareApi.Exceptions;
using TextShareApi.Extensions;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;

namespace TextShareApi.Controllers;

[ValidateModelState]
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
        if (IsEmail(registerDto.UserName))
            return BadRequest(new ExceptionDto {
                Code = "ValidationFailed",
                Description = "One or more validation errors occurred.",
                Details = [$"The Field {nameof(registerDto.UserName)} cannot represent an email."]
            });

        var result = await _accountService.Register(
            registerDto.UserName, registerDto.Email, registerDto.Password);

        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        var (user, token) = result.Value;
        
        return Ok(new UserWithTokenDto {
            UserName = user.UserName,
            Email = user.Email,
            Token = token
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
        var result = await _accountService.Login(loginDto.UserNameOrEmail, loginDto.Password);

        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

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
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(input, emailPattern);
    }
}