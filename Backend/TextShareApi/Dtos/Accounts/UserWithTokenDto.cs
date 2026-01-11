namespace TextShareApi.Dtos.Accounts;

public sealed class UserWithTokenDto {
    public string Id { get; set; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
}