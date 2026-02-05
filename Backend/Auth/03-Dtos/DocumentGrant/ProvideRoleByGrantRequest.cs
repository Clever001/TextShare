namespace Auth.Dto.DocumentGrant;

public class ProvideRoleByGrantRequest {
    public string DocumentGrantId {get; init;} = default!;
    public string CallingUserId {get; init;} = default!;
}