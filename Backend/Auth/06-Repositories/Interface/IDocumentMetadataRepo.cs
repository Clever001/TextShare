using Auth.Model;

namespace Auth.Repository.Interface;

public interface IDocumentMetadataRepo {
    Task<DocumentMetadata> SaveMetadataAboutDocument(DocumentMetadata document);
    Task<DocumentMetadata> GetMetadataAboutDocument(string documentId);
    Task<DocumentMetadata> UpdateDefaultRoleForDocument(string documentId, string roleName);
    Task DeleteMetadataAboutDocument(string documentId);
}