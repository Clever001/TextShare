namespace Auth.Dto.Document;

public record UserRoleRequest(
    string UserId,
    string DocumentId
);
