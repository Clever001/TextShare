namespace Auth.Dto.DocumentGrant;

public record ProvideRoleByGrantRequest(
    string DocumentGrantId,
    string CallingUserId
);
