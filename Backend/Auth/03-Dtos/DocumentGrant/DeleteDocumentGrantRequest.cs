namespace Auth.Dto.DocumentGrant;

public record DeleteDocumentGrantRequest(
    string DocumentGrantId,
    string CallingUserId
);