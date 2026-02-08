namespace Auth.Dto.DocumentGrant;

public record ProvideRoleByRoleNameRequest(
    string DocumentId,
    string RoleName,
    string UserIdToProvideRole,
    string CallingUserId
);
