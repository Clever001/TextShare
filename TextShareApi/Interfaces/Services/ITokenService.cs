using TextShareApi.Models;

namespace TextShareApi.Interfaces;

public interface ITokenService {
    string CreateToken(AppUser user);
}