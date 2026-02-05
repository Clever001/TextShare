using Auth.Model;

namespace Auth.Repository.Interface;

public interface IDocumentRoleRepository {
    Task<DocumentRole> CreateDocumentRole(DocumentRole role);
    Task<DocumentRole> GetDocumentRole(string roleName);
    Task<DocumentRole> UpdateDocumentRole(string roleName, DocumentRole documentRole);
    Task DeleteDocumentRole(string roleName);
}