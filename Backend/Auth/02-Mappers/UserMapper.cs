using Auth.Dto.Account;
using Auth.Grpc;
using Auth.Model;
using Auth.Other;

namespace Auth.Mapper;

public static class UserMapper {
    public static UserWithTokenGrpcDto ConvertToGrpcDto(UserWithTokenDto userTokenDto) {
        return new UserWithTokenGrpcDto() {
            User = new UserWithoutTokenGrpcDto() {
                Id = userTokenDto.User.Id,
                UserName = userTokenDto.User.UserName,
                Email = userTokenDto.User.Email
            },
            Token = userTokenDto.Token
        };
    }

    public static PagedUsersWithoutTokenGrpc
    ConvertToGrpcDto(PaginatedResponse<User> users) {
        var grpcUsersDto = new PagedUsersWithoutTokenGrpc() {
            Pagination = SharedMapper.ConvertToGrpcDto(users)
        };
        grpcUsersDto.Items.AddRange(
            users.Items.Select(ConvertToGrpcDto)
        );
        return grpcUsersDto;
    }

    public static UserWithoutTokenGrpcDto ConvertToGrpcDto(User user) {
        return new UserWithoutTokenGrpcDto() {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };
    }
}