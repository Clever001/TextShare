using Auth.DbContext;
using Auth.Model;
using Auth.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Auth.Repository.Impl;

public class DocumentRoleRepository(
    AppDbContext dbContext
) : IDocumentRoleRepository {
    public async Task<DocumentRole?> FindByName(string roleName) {
        return await dbContext.DocumentRoles.FindAsync(roleName);
    }

    public async Task<bool> ContainsByName(string roleName) {
        return await dbContext.DocumentRoles.AnyAsync(
            r => r.Name == roleName
        );
    }
}