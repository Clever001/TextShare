using TextShareApi.ClassesLib;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
using TextShareApi.Models;

namespace TextShareApi.Services;

public class FriendService : IFriendService {
    private readonly IFriendPairRepository _fpRepo;
    private readonly AccountRepository _accountRepository;

    public FriendService(IFriendPairRepository fpRepo, AccountRepository accountRepository) {
        _fpRepo = fpRepo;
        _accountRepository = accountRepository;
    }
    
    public async Task<Result> AddFriend(string firstUserName, string secondUserName) {
        if (firstUserName == secondUserName) {
            return Result.Failure("First and second users cannot be same", true);
        }

        var idsResult = await GetIds(firstUserName, secondUserName);
        if (!idsResult.IsSuccess) {
            return Result.Failure(idsResult.Error, idsResult.IsClientError);
        }
        var (firstId, secondId) = idsResult.Value;
        
        bool areFriends = await _fpRepo.ContainsFriendPair(firstId, secondId);
        if (areFriends) {
            return Result.Failure("User is already in friends list", true);
        }
        
        await _fpRepo.CreateFriendPairs(firstId, secondId);
        return Result.Success();
    }

    public async Task<Result<List<AppUser>>> GetFriends(string userName) {
        var userId = await _accountRepository.GetAccountId(userName);

        if (userId == null) {
            return Result<List<AppUser>>.Failure("Current user not found", true);
        }

        List<AppUser> friends =
            (await _fpRepo.GetFriendPairs(fp => fp.FirstUserId == userId))
            .Select(fp => fp.SecondUser)
            .ToList();

        return Result<List<AppUser>>.Success(friends);
    }

    public async Task<Result> RemoveFriend(string firstUserName, string secondUserName) {
        if (firstUserName == secondUserName) {
            return Result.Failure("First and second user name cannot be same", true);
        }
        
        var idsResult = await GetIds(firstUserName, secondUserName);
        if (!idsResult.IsSuccess) {
            return Result.Failure(idsResult.Error, idsResult.IsClientError);
        }
        var (firstId, secondId) = idsResult.Value;
        
        bool isDeleted = await _fpRepo.DeleteFriendPairs(firstId, secondId);
        if (!isDeleted) {
            return Result.Failure("User is not in friends list", true);
        }
        
        return Result.Success();
    }

    public async Task<Result<bool>> AreFriends(string firstUserName, string secondUserName) {
        if (firstUserName == secondUserName) {
            return Result<bool>.Failure("First and second user name cannot be same", true);
        }
        
        var idsResult = await GetIds(firstUserName, secondUserName);
        if (!idsResult.IsSuccess) {
            return Result<bool>.Failure(idsResult.Error, idsResult.IsClientError);
        }
        var (firstId, secondId) = idsResult.Value;
        
        bool areFriends = await _fpRepo.ContainsFriendPair(firstId, secondId);
        return Result<bool>.Success(areFriends);
    }

    private async Task<Result<(string, string)>> GetIds(string firstUserName, string secondUserName) {
        var firstId = await _accountRepository.GetAccountId(firstUserName);
        var secondId = await _accountRepository.GetAccountId(secondUserName);
        
        if (firstId == null) {
            return Result<(string, string)>.Failure("Couldn't find current user", false);
        }

        if (secondId == null) {
            return Result<(string, string)>.Failure("Couldn't find user to add to friends list", true);
        }
        
        return Result<(string, string)>.Success((firstId, secondId));
    }
}