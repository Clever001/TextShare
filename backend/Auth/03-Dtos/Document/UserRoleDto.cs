namespace Auth.Dto.Document;

public record UserRoleDto(
    string RoleName,
    bool CanRead,
    bool CanEdit,
    bool CanCreateRoleGrants,
    bool CanManageRoles
);