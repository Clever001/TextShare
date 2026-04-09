using Auth.Model;
using Auth.Other;

namespace Auth.Repository.Interface;

public interface IDocumentGrantRepository {
    Task CreateDocumentGrant(DocumentRoleGrant grant);
    Task<DocumentRoleGrant?> GetDocumentGrantById(string grantId);
    Task<bool> ContainsDocumentGrantById(string grantId);
    Task<SelectionOfItems<DocumentRoleGrant>> GetDocumentGrants<KeyOrderT>(
        QueryFilter<DocumentRoleGrant, KeyOrderT> queryFilter
    );
    Task UpdateDocumentGrant(DocumentRoleGrant grant);
    Task DeleteDocumentGrant(DocumentRoleGrant grant);
}