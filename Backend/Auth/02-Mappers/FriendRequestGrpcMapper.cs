using Auth.Grpc;
using Auth.Models;
using Auth.Other;
using Shared;

namespace Auth.Mappers;

public class FriendRequestGrpcMapper(
    SharedMapper _sharedMapper
) {
    public FriendRequestData ToFriendRequestData(FriendRequest friendRequest) {
        var frd =  new FriendRequestData {
            SenderId = friendRequest.SenderId,
            RecipientId = friendRequest.RecipientId,
        };
        if (friendRequest.IsAccepted != null) {
            frd.IsAccepted = friendRequest.IsAccepted.Value;
        }
        return frd;
    }

    public FRResult ToFRResult(Result<FriendRequest> result) {
        if (result.IsSuccess) {
            return new FRResult {
                FriendRequest = ToFriendRequestData(result.Value)
            };
        } else {
            return new FRResult {
                Exception = _sharedMapper.ToException(result.Exception)
            };
        }
    }

    public PaginatedFRs ToPaginatedFRs (PaginatedResponse<FriendRequest> response) {
        var friendRequests = new PaginatedFRs {
            Pagination = _sharedMapper.ToPaginationResponse(response)
        };
        if (response.Items != null && response.Items.Count > 0) {
            friendRequests.Items.AddRange(response.Items.Select(ToFriendRequestData));
        }
        return friendRequests;
    }

    public PaginatedFRsResult ToPaginatedFRsResult(Result<PaginatedResponse<FriendRequest>> result) {
        if (result.IsSuccess) {
            return new PaginatedFRsResult {
                FriendRequests = ToPaginatedFRs(result.Value)
            };
        } else {
            return new PaginatedFRsResult {
                Exception = _sharedMapper.ToException(result.Exception)
            };
        }
    }
}