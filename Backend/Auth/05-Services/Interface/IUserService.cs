using Auth.Dto.Account;
using Auth.Model;
using Auth.Other;
using Shared;

namespace Auth.Service.Interface;

public interface IUserService {
    Task<Result<UserWithTokenDto>> RegisterUser(RegisterUserDto registerDto);
    Task<Result<UserWithTokenDto>> LoginUser(LoginUserDto loginDto);
    Task<Result<UserWithTokenDto>> UpdateUser(UpdateUserDto updateDto);
    Task<Result<PaginatedResponse<User>>> GetUsers (UsersPagedFilterDto usersFilter);
}