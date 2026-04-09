namespace TextShareApi.Dtos.Accounts;

public sealed class UserWithoutTokenDto {
    public string Id { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
}