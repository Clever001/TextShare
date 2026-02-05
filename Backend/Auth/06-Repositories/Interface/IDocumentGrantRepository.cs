using Auth.Model;

namespace Auth.Repository.Interface;

public interface IDocumentGrantRepository {
    Task CreateDocumentRoleGrant(DocumentRoleGrant grant);
    Task<DocumentRoleGrant> GetDocumentRoleGrant(string grantId);
    Task UpdateDocumentRoleGrant(string grantId, DocumentRoleGrant grant);
    Task DeleteDocumentRoleGrant(string grantId);
}