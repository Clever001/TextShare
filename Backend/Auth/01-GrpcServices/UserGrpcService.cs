using Grpc.Core;
using Auth.Grpc;
using Auth.Dto.Account;
using Auth.Mapper;
using Auth.Service.Interface;
namespace Auth.GrpcService;

public class UserGrpcService(
    IUserService userService,
    ILogger<UserGrpcService> logger
) : UserGrpc.UserGrpcBase {
    public override async Task<UserWithTokenGrpcResult> RegisterUser(
        RegisterUserGrpcDto request, 
        ServerCallContext context
    ) {
        var registerDto = new RegisterUserDto(
            request.Name, request.Email, request.Password
        );

        try {
            var registrationResult = await userService.RegisterUser(registerDto);
            return SharedMapper.ConvertToGrpcResult(
                serviceOpResult: registrationResult,
                onSuccess: (convertable) => new UserWithTokenGrpcResult() {
                    User = UserMapper.ConvertToGrpcDto(convertable)
                },
                onFailure: (apiError) => new UserWithTokenGrpcResult() {
                    Exception = SharedMapper.ConvertToGrpcDto(apiError)
                }
            );
        } catch (Exception ex) {
            SharedGrpcServiceUtils.LogException(logger, ex);
            throw;
        }
    }

    public override async Task<UserWithTokenGrpcResult> LoginUser(
        LoginUserGrpcDto request, 
        ServerCallContext context
    ) {
        var loginDto = new LoginUserDto(
            request.NameOrEmail, request.Password
        );

        try {
            var loginResult = await userService.LoginUser(loginDto);
            return SharedMapper.ConvertToGrpcResult(
                serviceOpResult: loginResult,
                onSuccess: (convertable) => new UserWithTokenGrpcResult() {
                    User = UserMapper.ConvertToGrpcDto(convertable)
                },
                onFailure: (apiError) => new UserWithTokenGrpcResult() {
                    Exception = SharedMapper.ConvertToGrpcDto(apiError)
                }
            );
        } catch (Exception ex) {
            SharedGrpcServiceUtils.LogException(logger, ex);
            throw;
        }
    }

    public override async Task<UserWithTokenGrpcResult> UpdateUser(
        UpdateUserGrpcDto request, 
        ServerCallContext context
    ) {
        var updateDto = new UpdateUserDto(
            request.InitialUserId, 
            request.HasNewName ? request.NewName : null, 
            request.HasNewEmail ? request.NewEmail : null
        );

        try {
            var updateResult = await userService.UpdateUser(updateDto);
            return SharedMapper.ConvertToGrpcResult(
                serviceOpResult: updateResult,
                onSuccess: (convertable) => new UserWithTokenGrpcResult() {
                    User = UserMapper.ConvertToGrpcDto(convertable)
                },
                onFailure: (apiError) => new UserWithTokenGrpcResult() {
                    Exception = SharedMapper.ConvertToGrpcDto(apiError)
                }
            );
        } catch (Exception ex) {
            SharedGrpcServiceUtils.LogException(logger, ex);
            throw;
        }
    }

    public override async Task<PagedUsersWithoutTokenGrpcResult> GetUsers(
        UsersGrpcPagedFilter request, 
        ServerCallContext context
    ) {
        var usersFilter = new UsersPagedFilterDto(
            request.HasUserName ? request.UserName : null, 
            request.PaginationPage.PageNumber, 
            request.PaginationPage.PageSize
        );

        try {
            var usersCollectionResult = await userService.GetUsers(usersFilter);
            return SharedMapper.ConvertToGrpcResult(
                serviceOpResult: usersCollectionResult,
                onSuccess: (convertable) => new PagedUsersWithoutTokenGrpcResult() {
                    Users = UserMapper.ConvertToGrpcDto(convertable)
                },
                onFailure: (apiError) => new PagedUsersWithoutTokenGrpcResult() {
                    Exception = SharedMapper.ConvertToGrpcDto(apiError)
                }
            );
        } catch (Exception ex) {
            SharedGrpcServiceUtils.LogException(logger, ex);
            throw;
        }
    }
}