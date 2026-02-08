namespace Auth.Dto.Document;

public record UpdateDefaultRoleRequest(
    string DocumentId,
    string DefaultRoleName
);
