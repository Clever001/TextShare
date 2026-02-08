using Auth.Model;
using Microsoft.AspNetCore.Identity;

namespace Auth.Repository.Interface;

public interface IDocumentRoleRepository {
    Task<IdentityResult> CreateDocumentRole(DocumentRole role);
    Task<DocumentRole> GetDocumentRole(string roleName);
    Task<DocumentRole> UpdateDocumentRole(string roleName, DocumentRole documentRole);
    Task DeleteDocumentRole(string roleName);
}