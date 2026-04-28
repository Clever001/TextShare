using DocShareApi.Data;
using DocShareApi.Dtos.Enums;
using DocShareApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DocShareApi.Repositories;

public class DevRolesRepo(
    AppDbContext context
) : IDevRolesRepo {
    public async Task<UserDevRole?> GetUserToDocRole(string userId, string documentId) {
        return (await context.UserToDocRoles.FindAsync(userId, documentId))?.Role;
    }

    public async Task AssignRoleToUser(string userId, string documentId, UserDevRole role) {
        var foundItem = await context.UserToDocRoles.FindAsync(userId, documentId);
        if (foundItem == null) {
            var newRole = new UserToDocRole() {
                UserId = userId,
                DocumentId = documentId,
                Role = role
            };
            await context.UserToDocRoles.AddAsync(newRole);
            await context.SaveChangesAsync();
        } else {
            foundItem.Role = role;
            context.UserToDocRoles.Update(foundItem);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteAllForDocument(string documentId) {
        await context.UserToDocRoles
            .Where(r => r.DocumentId == documentId)
            .ExecuteDeleteAsync();
    }
}