using Auth.Grpc;
using Auth.Mappers;
using Auth.Other;
using Auth.Services.Interfaces;
using Grpc.Core;

namespace Auth.Services;

public class FriendGrpcService(
    IFriendService _friendService,
    FriendGrpcMapper _mapper,
    SharedMapper _sharedMapper
) : FriendGrpc.FriendGrpcBase {
    public override async Task<PaginatedFriendsResult> GetFriends(FriendsFilter request, ServerCallContext context) {
        var pagination = _sharedMapper.ToPaginationCommand(request.PaginationPage);
        var result = await _friendService.GetFriends(
            pagination, 
            request.IsAscending, 
            request.HasFriendName ? request.FriendName : null, 
            request.SenderName
        );
        return _mapper.ToPaginatedFriendsResult(result);
    }

    public override async Task<EmptyResult> RemoveFriend(RemoveFriendReq request, ServerCallContext context) {
        var result = await _friendService.RemoveFriend(request.FirseUserName, request.SecondUserName);
        return _sharedMapper.ToEmptyResult(result);
    }

    public override async Task<BooleanResult> AreFriendsByName(AreFriendsByNameReq request, ServerCallContext context) {
        var result = await _friendService.AreFriendsByName(request.FirstUserName, request.SecondUserName);
        return _sharedMapper.ToBooleanResult(result);
    }

    public override async Task<BooleanResult> AreFriendsById(AreFriendsByIdReq request, ServerCallContext context) {
        var result = await _friendService.AreFriendsById(request.FirstUserId, request.SecondUserId);
        return _sharedMapper.ToBooleanResult(result);
    }
}