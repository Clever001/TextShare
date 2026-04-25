using DocShareApi.Models;

namespace DocShareApi.Interfaces.Services;

public interface ITokenService {
    string CreateToken(AppUser user);
}