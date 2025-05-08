using TextShareApi.ClassesLib;
using TextShareApi.Exceptions;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
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

    public async Task<Result<List<AppUser>>> GetFriends(string userName) {
        var userId = await _accountRepository.GetAccountId(userName);

        if (userId == null) return Result<List<AppUser>>.Failure(new NotFoundException("Current user not found."));

        var friends =
            (await _fpRepo.GetFriendPairs(fp => fp.FirstUserId == userId))
            .Select(fp => fp.SecondUser)
            .ToList();

        return Result<List<AppUser>>.Success(friends);
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

    public async Task<Result<List<string>>> GetFriendsIds(string userId) {
        var ids = (await _fpRepo.GetFriendPairs(p => p.FirstUserId == userId))
            .Select(p => p.SecondUserId).ToList();
        return Result<List<string>>.Success(ids);
    }
}