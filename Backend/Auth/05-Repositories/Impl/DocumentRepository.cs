using Auth.DbContext;
using Auth.Model;
using Auth.Other;
using Auth.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Auth.Repository.Impl;

public class DocumentRepository(
    AppDbContext dbContext
) : IDocumentRepository {
    private readonly DbSet<DocumentMetadata> dbSet = dbContext.DocumentsMetadata;

    public async Task CreateDocumentMetadata(DocumentMetadata document) {
        await dbSet.AddAsync(document);
    }

    public async Task<DocumentMetadata?> FindById(string documentId) {
        return await dbSet.FindAsync(documentId);
    }

    public async Task<bool> ContainsById(string documentId) {
        return await dbSet.AnyAsync(
            d => d.Id == documentId
        );
    }

    public async Task<SelectionOfItems<DocumentMetadata>> 
    GetDocumentsMetadata<KeyOrderT>(
        QueryFilter<DocumentMetadata, KeyOrderT> queryFilter
    ) {
        var (countOfDocuments, documents) = 
            await queryFilter.ApplyFilter(dbSet);

        return new SelectionOfItems<DocumentMetadata>(
            TotalCount: countOfDocuments,
            Selection: await documents.ToArrayAsync()
        );
    }

    public async Task UpdateDefaultRoleForDocument(DocumentMetadata document) {
        dbSet.Update(document);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteMetadataAboutDocument(DocumentMetadata document) {
        dbSet.Remove(document);
        await dbContext.SaveChangesAsync();
    }
}