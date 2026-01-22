using Auth.Grpc;
using Auth.Models;
using Auth.Other;
using Shared;

namespace Auth.Mappers;

public class FriendGrpcMapper(
    SharedMapper _sharedMapper
) {
    public PaginatedFriends ToPaginatedFriends(PaginatedResponse<AppUser> response) {
        var friends = new PaginatedFriends {
            Pagination = _sharedMapper.ToPaginationResponse(response)
        };
        if (response.Items != null && response.Items.Count > 0) {
            friends.Items.AddRange(response.Items.Select(u => new UserWithoutToken {
                Id = u.Id,
                UserName = u.UserName
            }));
        }
        return friends;
    }

    public PaginatedFriendsResult ToPaginatedFriendsResult(Result<PaginatedResponse<AppUser>> result) {
        if (result.IsSuccess) {
            return new PaginatedFriendsResult {
                Friends = ToPaginatedFriends(result.Value)
            };
        } else {
            return new PaginatedFriendsResult {
                Exception = _sharedMapper.ToException(result.Exception)
            };
        }
    }
}