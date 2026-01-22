namespace Auth.Other;

public sealed class UpdateUserCommand {
    public string? UserName { get; set; } = null;

    public string? Email { get; set; } = null;
}