using Auth.Model;

namespace Auth.Repository.Interface;

public interface IDocumentRoleRepository {
    Task<DocumentRole?> FindByName(string roleName);
    Task<bool> ContainsByName(string roleName);
}