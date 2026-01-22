using Grpc.Core;
using Auth.Grpc;
using Auth.Services.Interfaces;
using Auth.Mappers;
using Auth.Other;

namespace Auth.Services;

public class AccountGrpcService(
    IAccountService _accountService,
    AccountGrpcMapper _mapper
) : AccountGrpc.AccountGrpcBase {
    public override async Task<UserResult> RegisterUser(RegisterUserReq request, ServerCallContext context) {
        var result = await _accountService.Register(request.Name, request.Email, request.Password);
        return _mapper.ToUserResult(result);
    }

    public override async Task<UserResult> LoginUser(LoginReq request, ServerCallContext context) {
        var result = await _accountService.Login(request.NameOrEmail, request.Password);
        return _mapper.ToUserResult(result);
    }

    public override async Task<UserResult> UpdateUser(UpdateUserReq request, ServerCallContext context) {
        var command = _mapper.ToUpdateUserCommand(request);
        var result = await _accountService.Update(request.InitialUserName, command);
        return _mapper.ToUserResult(result);
    }

    public override async Task<PaginatedUsersResult> GetUsers(UsersFilter request, ServerCallContext context) {
        var pagination = new PaginationCommand {
            PageNumber = request.PaginationPage.PageNumber,
            PageSize = request.PaginationPage.PageSize
        };
        var result = await _accountService.GetUsers(pagination, request.HasUserName ? request.UserName : null);
        return _mapper.ToPaginatedUsersResult(result);
    }
}