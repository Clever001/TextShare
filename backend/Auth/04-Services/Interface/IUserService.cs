using Auth.Dto.Account;
using Auth.Model;
using Auth.Other;
using Shared.Result;

namespace Auth.Service.Interface;

public interface IUserService {
    Task<ApiResult<UserWithTokenDto>> RegisterUser(RegisterUserRequest registerDto);
    Task<ApiResult<UserWithTokenDto>> LoginUser(LoginUserRequest loginDto);
    Task<ApiResult<UserWithTokenDto>> UpdateUser(UpdateUserRequest updateDto);
    Task<ApiResult<PaginatedResponse<User>>> GetUsers (UsersPagedFilter usersFilter);
}