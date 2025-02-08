using TextShareApi.Dtos.Accounts;
using TextShareApi.Models;

namespace TextShareApi.Mappers;

public static class UserMapper {
    public static UserDto ToNewUserDto(this AppUser user, string token) {
        return new() {
            UserName = user.UserName,
            Email = user.Email,
            Token = token,
        };
    }
}