using Auth.DbContext;
using Auth.Model;
using Auth.Other;
using Auth.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Auth.Repository.Impl;

public class DocumentGrantRepository(
    AppDbContext dbContext
) : IDocumentGrantRepository {
    private readonly DbSet<DocumentRoleGrant> dbSet = dbContext.DocumentRoleGrants;
    
    public async Task CreateDocumentGrant(DocumentRoleGrant grant) {
        await dbSet.AddAsync(grant);
    }

    public async Task<DocumentRoleGrant?> GetDocumentGrantById(string grantId) {
        return await dbSet.FindAsync(grantId);
    }

    public async Task<bool> ContainsDocumentGrantById(string grantId) {
        return await dbSet.AnyAsync(
            g => g.Id == grantId
        );
    }

    public async Task<SelectionOfItems<DocumentRoleGrant>> 
    GetDocumentGrants<KeyOrderT>(
        QueryFilter<DocumentRoleGrant, KeyOrderT> queryFilter
    ) {
        var (countOfGrants, grants) = 
            await queryFilter.ApplyFilter(dbSet);
        
        return new SelectionOfItems<DocumentRoleGrant>(
            TotalCount: countOfGrants,
            Selection: await grants.ToArrayAsync()
        );
    }

    public async Task UpdateDocumentGrant(DocumentRoleGrant grant) {
        dbSet.Update(grant);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteDocumentGrant(DocumentRoleGrant grant) {
        dbSet.Remove(grant);
        await dbContext.SaveChangesAsync();
    }
}