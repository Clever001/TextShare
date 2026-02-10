using Auth.Model;

namespace Auth.Service.Interface;

public interface ITokenService {
    string CreateToken(User user);
}