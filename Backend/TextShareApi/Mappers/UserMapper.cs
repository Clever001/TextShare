using TextShareApi.Dtos.Accounts;
using TextShareApi.Models;

namespace TextShareApi.Mappers;

public static class UserMapper {
    public static UserWithTokenDto ToUserWithTokenDto(this AppUser user, string token) {
        return new UserWithTokenDto {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            Token = token
        };
    }

    public static UserWithoutTokenDto ToUserWithoutTokenDto(this AppUser user) {
        return new UserWithoutTokenDto {
            Id = user.Id,
            UserName = user.UserName!
        };
    }
}