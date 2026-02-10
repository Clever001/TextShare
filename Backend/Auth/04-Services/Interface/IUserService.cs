using Auth.Dto.Account;
using Auth.Model;
using Auth.Other;
using Shared.Result;

namespace Auth.Service.Interface;

public interface IUserService {
    Task<ApiResult<UserWithTokenDto>> RegisterUser(RegisterUserDto registerDto);
    Task<ApiResult<UserWithTokenDto>> LoginUser(LoginUserDto loginDto);
    Task<ApiResult<UserWithTokenDto>> UpdateUser(UpdateUserDto updateDto);
    Task<ApiResult<PaginatedResponse<User>>> GetUsers (UsersPagedFilterDto usersFilter);
}