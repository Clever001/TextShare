using Auth.Model;
using Auth.Other;

namespace Auth.Repository.Interface;

public interface IDocumentRepository {
    Task CreateDocumentMetadata(DocumentMetadata document);
    Task<DocumentMetadata?> FindById(string documentId);
    Task<bool> ContainsById(string documentId);
    Task<SelectionOfItems<DocumentMetadata>> GetDocumentsMetadata<KeyOrderT>(
        QueryFilter<DocumentMetadata, KeyOrderT> queryFilter
    );
    Task UpdateDefaultRoleForDocument(DocumentMetadata document);
    Task DeleteMetadataAboutDocument(DocumentMetadata document);
}