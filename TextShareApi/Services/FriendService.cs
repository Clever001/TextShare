using System.Linq.Expressions;
using TextShareApi.ClassesLib;
using TextShareApi.Dtos.QueryOptions;
using TextShareApi.Exceptions;
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

    public async Task<Result> AddFriend(string firstUserName, string secondUserName) {
        if (firstUserName == secondUserName) return Result.Failure(new BadRequestException("First and second users cannot be same."));
        
        var (firstId, secondId) = await _accountRepository.GetAccountIds(firstUserName, secondUserName);
        if (firstId == null) {
            return Result.Failure(new NotFoundException("First user not found."));
        }
        if (secondId == null) {
            return Result.Failure(new NotFoundException("Second user not found."));
        }

        var areFriends = await _fpRepo.ContainsFriendPair(firstId, secondId);
        if (areFriends) return Result.Failure(new BadRequestException("User is already in friends list."));

        await _fpRepo.CreateFriendPairs(firstId, secondId);
        return Result.Success();
    }

    public async Task<Result<PaginatedResponseDto<AppUser>>> GetFriends(PaginationDto pagination, bool isAscending, string? friendName, string senderName) {
        int skip = (pagination.PageNumber - 1) * pagination.PageSize;
        int take = pagination.PageSize;
        
        var senderId = await _accountRepository.GetAccountId(senderName);
        var predicates = new List<Expression<Func<FriendPair, bool>>> {
            p => p.FirstUserId == senderId
        };
        if (friendName != null && friendName.Trim() != "")
            predicates.Add(p => p.SecondUser.UserName!.ToLower().Contains(friendName.ToLower()));

        var (count, pairs) = await _fpRepo.GetFriendPairs(
            skip: skip,
            take: take,
            keyOrder: p => p.SecondUser.UserName,
            isAscending: isAscending,
            predicates: predicates,
            includeSender: false,
            includeRecipient: true
        );
        
        var friends = pairs.Select(p => p.SecondUser).ToList();

        return Result<PaginatedResponseDto<AppUser>>.Success(friends.ToPaginatedResponse(pagination, count));
    }

    public async Task<Result> RemoveFriend(string firstUserName, string secondUserName) {
        if (firstUserName == secondUserName) return Result.Failure(new BadRequestException("First and second user name cannot be same."));

        var (firstId, secondId) = await _accountRepository.GetAccountIds(firstUserName, secondUserName);
        if (firstId == null) {
            return Result.Failure(new NotFoundException("First user not found."));
        }
        if (secondId == null) {
            return Result.Failure(new NotFoundException("Second user not found."));
        }

        var isDeleted = await _fpRepo.DeleteFriendPairs(firstId, secondId);
        if (!isDeleted) return Result.Failure(new BadRequestException("User is not in friends list."));

        return Result.Success();
    }

    public async Task<Result<bool>> AreFriendsByName(string firstUserName, string secondUserName) {
        if (firstUserName == secondUserName)
            return Result<bool>.Failure(new BadRequestException("First and second user name cannot be same."));

        var (firstId, secondId) = await _accountRepository.GetAccountIds(firstUserName, secondUserName);
        if (firstId == null) {
            return Result<bool>.Failure(new NotFoundException("First user not found."));
        }
        if (secondId == null) {
            return Result<bool>.Failure(new NotFoundException("Second user not found."));
        }

        var areFriends = await _fpRepo.ContainsFriendPair(firstId, secondId);
        return Result<bool>.Success(areFriends);
    }

    public async Task<Result<bool>> AreFriendsById(string firstUserId, string secondUserId) {
        return Result<bool>.Success(await _fpRepo.ContainsFriendPair(firstUserId, secondUserId));
    }
}