using DocShareApi.Dtos.Enums;
using DocShareApi.Models;

namespace DocShareApi.Repositories;

public interface IDevRolesRepo {
    Task<UserDevRole?> GetUserToDocRole(string userId, string documentId);
    Task AssignRoleToUser(string userId, string documentId, UserDevRole role);
    Task DeleteAllForDocument(string documentId);
}