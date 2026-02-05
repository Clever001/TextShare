namespace Auth.Dto.DocumentGrant;

public class ProvideRoleByRoleNameRequest {
    public string DocumenrId {get; init;} = default!;
    public string RoleName {get; init;} = default!;
    public string CallingUserId {get; init;} = default!;
}