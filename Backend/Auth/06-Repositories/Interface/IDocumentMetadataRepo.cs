using Auth.Model;
using Microsoft.AspNetCore.Identity;

namespace Auth.Repository.Interface;

public interface IDocumentMetadataRepository {
    Task<IdentityResult> SaveMetadataAboutDocument(DocumentMetadata document);
    Task<DocumentMetadata> GetMetadataAboutDocument(string documentId);
    Task<DocumentMetadata> UpdateDefaultRoleForDocument(string documentId, string roleName);
    Task<IdentityResult> DeleteMetadataAboutDocument(string documentId);
}