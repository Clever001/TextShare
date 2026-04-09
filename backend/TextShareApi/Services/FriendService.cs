using System.Linq.Expressions;
using Shared;
using TextShareApi.Dtos.QueryOptions;
using Shared.ApiError;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;
using TextShareApi.Models;

namespace TextShareApi.Services;

public class FriendService : IFriendService {
    private readonly IAccountRepository _accountRepository;
    private readonly IFriendPairRepository _fpRepo;

    public FriendService(IFriendPairRepository fpRepo, IAccountRepository accountRepository) {
        _fpRepo = fpRepo;
        _accountRepository = accountRepository;
    }

    // TODO: Check if this method needed.
    public async Task<Result> AddFriend(string firstUserName, string secondUserName) {
        if (firstUserName == secondUserName)
            return Result.Failure(new BadRequestApiError("First and second users cannot be same."));

        var (firstId, secondId) = await _accountRepository.GetAccountIds(firstUserName, secondUserName);
        if (firstId == null) return Result.Failure(new NotFoundApiError("First user not found."));
        if (secondId == null) return Result.Failure(new NotFoundApiError("Second user not found."));

        var areFriends = await _fpRepo.ContainsFriendPair(firstId, secondId);
        if (areFriends) return Result.Failure(new BadRequestApiError("User is already in friends list."));

        await _fpRepo.CreateFriendPairs(firstId, secondId);
        return Result.Success();
    }

    public async Task<Result<PaginatedResponseDto<AppUser>>> GetFriends(PaginationDto pagination, bool isAscending,
        string? friendName, string senderName) {
        var skip = (pagination.PageNumber - 1) * pagination.PageSize;
        var take = pagination.PageSize;

        var senderId = await _accountRepository.GetAccountId(senderName);
        var predicates = new List<Expression<Func<FriendPair, bool>>> {
            p => p.FirstUserId == senderId
        };
        if (friendName != null && friendName.Trim() != "")
            predicates.Add(p => p.SecondUser.UserName!.ToLower().Contains(friendName.ToLower()));

        var (count, pairs) = await _fpRepo.GetFriendPairs(
            skip,
            take,
            p => p.SecondUser.UserName,
            isAscending,
            predicates,
            false,
            true
        );

        var friends = pairs.Select(p => p.SecondUser).ToList();

        return Result<PaginatedResponseDto<AppUser>>.Success(friends.ToPaginatedResponse(pagination, count));
    }

    public async Task<Result> RemoveFriend(string firstUserName, string secondUserName) {
        if (firstUserName == secondUserName)
            return Result.Failure(new BadRequestApiError("First and second user name cannot be same."));

        var (firstId, secondId) = await _accountRepository.GetAccountIds(firstUserName, secondUserName);
        if (firstId == null) return Result.Failure(new NotFoundApiError("First user not found."));
        if (secondId == null) return Result.Failure(new NotFoundApiError("Second user not found."));

        var isDeleted = await _fpRepo.DeleteFriendPairs(firstId, secondId);
        if (!isDeleted) return Result.Failure(new BadRequestApiError("User is not in friends list."));

        return Result.Success();
    }

    public async Task<Result<bool>> AreFriendsByName(string firstUserName, string secondUserName) {
        if (firstUserName == secondUserName)
            return Result<bool>.Failure(new BadRequestApiError("First and second user name cannot be same."));

        var (firstId, secondId) = await _accountRepository.GetAccountIds(firstUserName, secondUserName);
        if (firstId == null) return Result<bool>.Failure(new NotFoundApiError("First user not found."));
        if (secondId == null) return Result<bool>.Failure(new NotFoundApiError("Second user not found."));

        var areFriends = await _fpRepo.ContainsFriendPair(firstId, secondId);
        return Result<bool>.Success(areFriends);
    }

    public async Task<Result<bool>> AreFriendsById(string firstUserId, string secondUserId) {
        return Result<bool>.Success(await _fpRepo.ContainsFriendPair(firstUserId, secondUserId));
    }
}