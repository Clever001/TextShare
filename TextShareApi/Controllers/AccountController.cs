using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.Attributes;
using TextShareApi.Dtos.Accounts;
using TextShareApi.Dtos.Exception;
using TextShareApi.Dtos.QueryOptions;
using TextShareApi.Dtos.QueryOptions.Filters;
using TextShareApi.Exceptions;
using TextShareApi.Extensions;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;
using TextShareApi.Models;

namespace TextShareApi.Controllers;

[ValidateModelState]
[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase {
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAccountService accountService,
        ILogger<AccountController> logger) {
        _accountService = accountService;
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
        
        return Ok(user.ToUserWithTokenDto(token));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
        var result = await _accountService.Login(loginDto.UserNameOrEmail, loginDto.Password);

        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        var (user, token) = result.Value;
        
        return Ok(user.ToUserWithTokenDto(token));
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationDto pagination,
        [FromQuery] string? userName) {
       
        var result = await _accountService.GetUsers(pagination, userName);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);
        
        return Ok(result.Value.Convert(u => u.ToUserWithoutTokenDto()));
    }

    private bool IsEmail(string input) {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(input, emailPattern);
    }
}