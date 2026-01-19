using Grpc.Core;
using Auth.Grpc; // Пространство имен, сгенерированное из .proto файла
using System.Threading.Tasks;

namespace Auth.Services;

public class AccountGrpcService : AccountGrpc.AccountGrpcBase {
    public override Task<UserResult> RegisterUser(RegisterUserReq request, ServerCallContext context) {
        return base.RegisterUser(request, context);
    }

    public override Task<UserResult> LoginUser(LoginReq request, ServerCallContext context) {
        return base.LoginUser(request, context);
    }

    public override Task<UserResult> UpdateUser(UpdateUserReq request, ServerCallContext context) {
        return base.UpdateUser(request, context);
    }

    public override Task<PaginatedUsersResult> GetUsers(UsersFilter request, ServerCallContext context) {
        return base.GetUsers(request, context);
    }
}