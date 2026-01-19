using Auth.Models;

namespace Auth.Services.Interfaces;

public interface ITokenService {
    string CreateToken(AppUser user);
}