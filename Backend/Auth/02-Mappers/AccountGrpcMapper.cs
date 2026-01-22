using Auth.Grpc;
using Auth.Models;
using Auth.Other;
using Shared;

namespace Auth.Mappers;

public class AccountGrpcMapper(
    SharedMapper _sharedMapper
) {
    public PaginatedUsersWithoutToken ToPaginatedUsersWithoutToken(PaginatedResponse<AppUser> users) {
        var grpcUsers = new PaginatedUsersWithoutToken {
            Pagination = _sharedMapper.ToPaginationResponse(users)
        };
        if (users.Items != null && users.Items.Count != 0) {
            grpcUsers.Items.AddRange(users.Items.Select(u => new UserWithoutToken {
                Id = u.Id,
                UserName = u.UserName
            }));
        }
        return grpcUsers;
    }

    public UpdateUserCommand ToUpdateUserCommand(UpdateUserReq request) {
        var result = new UpdateUserCommand {
            UserName = request.HasName ? request.Name : null,
            Email = request.HasEmail ? request.Email : null
        };
        return result;
    }

    public UserResult ToUserResult(Result<(AppUser, string)> result) {
        if (result.IsSuccess) {
            return new UserResult {
                User = _sharedMapper.ToUserWithToken(result.Value)
            };
        } else {
            return new UserResult {
                Exception = _sharedMapper.ToException(result.Exception)
            };
        }
    }

    public PaginatedUsersResult ToPaginatedUsersResult(Result<PaginatedResponse<AppUser>> result) {
        if (result.IsSuccess) {
            return new PaginatedUsersResult {
                Users = ToPaginatedUsersWithoutToken(result.Value)
            };
        } else {
            return new PaginatedUsersResult {
                Exception = _sharedMapper.ToException(result.Exception)
            };
        }
    }
}