using Auth.Grpc;
using Grpc.Core;

namespace Auth.Services;

public class FriendGrpcService : FriendGrpc.FriendGrpcBase {
    public override Task<PaginatedFriendsResult> GetFriends(FriendsFilter request, ServerCallContext context) {
        return base.GetFriends(request, context);
    }

    public override Task<EmptyResult> RemoveFriend(RemoveFriendReq request, ServerCallContext context) {
        return base.RemoveFriend(request, context);
    }

    public override Task<BooleanResult> AreFriendsByName(AreFriendsByNameReq request, ServerCallContext context) {
        return base.AreFriendsByName(request, context);
    }

    public override Task<BooleanResult> AreFriendsById(AreFriendsByIdReq request, ServerCallContext context) {
        return base.AreFriendsById(request, context);
    }
}