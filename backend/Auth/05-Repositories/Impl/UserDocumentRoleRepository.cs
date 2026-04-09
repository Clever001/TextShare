using Auth.DbContext;
using Auth.Model;
using Auth.Other;
using Auth.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Auth.Repository.Impl;

using Relation = UserDocumentRoleAssignment;

public class UserDocumentRoleRepository(
    AppDbContext dbContext
) : IUserDocumentRoleRepository {
    private readonly DbSet<Relation> dbSet = dbContext.UserDocumentRoleAssignments;

    public async Task CreateRelation(Relation relation) {
        await dbSet.AddAsync(relation);
    }

    public async Task<Relation?> GetRelation(string userId, string documentId) {
        return await dbSet.FindAsync(userId, documentId);
    }

    public async Task<bool> ContainsRelation(string userId, string documentId) {
        return await dbSet.AnyAsync(
            r => r.UserId == userId && r.TextId == documentId
        );
    }

    public async Task<SelectionOfItems<Relation>> 
    GetRelations<KeyOrderT>(
        QueryFilter<Relation, KeyOrderT> queryFilter
    ) {
        var (countOfRelations, relations) =
            await queryFilter.ApplyFilter(dbSet);

        return new SelectionOfItems<Relation>(
            TotalCount: countOfRelations,
            Selection: await relations.ToArrayAsync()
        );
    }

    public async Task UpdateRelation(Relation relation) {
        dbSet.Update(relation);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteRelation(Relation relation) {
        dbSet.Remove(relation);
        await dbContext.SaveChangesAsync();
    }
}