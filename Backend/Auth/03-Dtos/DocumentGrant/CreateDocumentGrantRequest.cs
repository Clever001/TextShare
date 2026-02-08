namespace Auth.Dto.DocumentGrant;

public record CreateDocumentGrantRequest(
    string DocumentId,
    string RoleName,
    string CallingUserId
);
