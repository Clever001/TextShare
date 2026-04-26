using DocShareApi.Models;

namespace DocShareApi.Services;

public interface ITokenService {
    string CreateToken(AppUser user);
}
