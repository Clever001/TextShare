using Auth.Model;
using Auth.Other;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Auth.Repository.Interface;

public interface IDocumentRepository {
    Task CreateDocumentMetadata(DocumentMetadata document);
    Task<DocumentMetadata?> FindById(string documentId);
    Task<bool> ContainsById(string documentId);
    Task<SelectionOfItems<DocumentMetadata>> GetDocumentsMetadata<KeyOrderT>(
        QueryFilter<DocumentMetadata, KeyOrderT> queryFilter
    );
    Task<EntityResult> UpdateDefaultRoleForDocument(string documentId, string defaultRoleName);
    Task<EntityResult> DeleteMetadataAboutDocument(string documentId);
}