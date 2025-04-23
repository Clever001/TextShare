using TextShareApi.Models;

namespace TextShareApi.Interfaces.Services;

public interface ITokenService {
    string CreateToken(AppUser user);
}