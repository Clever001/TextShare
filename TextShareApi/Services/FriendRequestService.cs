using System.Diagnostics;
using TextShareApi.ClassesLib;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
using TextShareApi.Models;

namespace TextShareApi.Services;

public class FriendRequestService : IFriendRequestService {
    private readonly IFriendRequestRepository _frRepo;
    private readonly IAccountRepository _accountRepository;
    private readonly IFriendService _friendService;

    public FriendRequestService(IFriendRequestRepository frRepo, IAccountRepository accountRepository,
        IFriendService friendService) {
        _frRepo = frRepo;
        _accountRepository = accountRepository;
        _friendService = friendService;
    }
    
    public async Task<Result<FriendRequest>> Create(string senderName, string recipientName) {
        if (senderName == recipientName) {
            return Result<FriendRequest>.Failure("Sender name and recipient name cannot be the same", true);
        }
        
        var idResult = await GetIds(senderName, recipientName);
        if (!idResult.IsSuccess) {
            return Result<FriendRequest>.Failure(idResult.Error, false);
        }
        var (senderId, recipientId) = idResult.Value;
        
        var areFriends = await _friendService.AreFriends(senderId, recipientId);
        Debug.Assert(areFriends.IsSuccess, $"Check of \"{nameof(areFriends)}\" was not successful");
        if (areFriends.Value) {
            return Result<FriendRequest>.Failure("Users are friends already", true);
        }
        bool exists = await _frRepo.ContainsRequest(senderId, recipientId);
        if (exists) {
            return Result<FriendRequest>.Failure("Already exists", true);
        }
        exists = await _frRepo.ContainsRequest(recipientId, senderId);
        if (exists) {
            return Result<FriendRequest>.Failure("Reverse request already exists", true);
        }
        
        var request = await _frRepo.CreateRequest(senderId, recipientId);
        return Result<FriendRequest>.Success(request);
    }

    public async Task<Result> Delete(string senderName, string recipientName) {
        if (senderName == recipientName) {
            return Result.Failure("Sender name and recipient name cannot be the same", true);
        }
        
        var idResult = await GetIds(senderName, recipientName);
        if (!idResult.IsSuccess) {
            return Result.Failure(idResult.Error, false);
        }
        var (senderId, recipientId) = idResult.Value;
        
        bool isDeleted = await _frRepo.DeleteRequest(senderId, recipientId);
        if (!isDeleted) {
            return Result.Failure("Did not exist from the beginning", true);
        }
        return Result.Success();
    }

    public async Task<Result<FriendRequest>> Process(string senderName, string recipientName, bool acceptRequest) {
        if (senderName == recipientName) {
            return Result<FriendRequest>.Failure("Sender name and recipient name cannot be the same", true);
        }
        
        var idResult = await GetIds(senderName, recipientName);
        if (!idResult.IsSuccess) {
            return Result<FriendRequest>.Failure(idResult.Error, false);
        }
        var (senderId, recipientId) = idResult.Value;
        
        var request = await _frRepo.GetRequest(senderId, recipientId);
        if (request == null) {
            return Result<FriendRequest>.Failure("Request does not exist", true);
        }

        if (acceptRequest) {
            var result = await _friendService.AddFriend(senderId, recipientId);
            if (!result.IsSuccess) {
                return Result<FriendRequest>.Failure($"Error while adding recipient to friends list: {result.Error}", false);
            }
            request.IsAccepted = true;
            bool deleted = await _frRepo.DeleteRequest(senderId, recipientId);
            Debug.Assert(deleted, $"Failed to delete request after adding friend");
            return Result<FriendRequest>.Success(request);
        }
        
        // Not accepted
        request = await _frRepo.UpdateRequest(senderId, recipientId, false);
        if (request is null) {
            return Result<FriendRequest>.Failure("Does not exist", true);
        }
        return Result<FriendRequest>.Success(request);
    }

    public async Task<Result<FriendRequest?>> GetFriendRequest(string senderName, string recipientName) {
        if (senderName == recipientName) {
            return Result<FriendRequest?>.Failure("Sender name and recipient name cannot be the same", true);
        }
        
        var idResult = await GetIds(senderName, recipientName);
        if (!idResult.IsSuccess) {
            return Result<FriendRequest?>.Failure(idResult.Error, false);
        }
        var (senderId, recipientId) = idResult.Value;
        
        var request = await _frRepo.GetRequest(senderId, recipientId);
        return Result<FriendRequest?>.Success(request);
    }

    public async Task<Result<List<FriendRequest>>> GetSentFriendRequests(string senderName) {
        var senderId = await _accountRepository.GetAccountId(senderName);
        if (senderId == null) {
            return Result<List<FriendRequest>>.Failure("Sender does not exist", false);
        }
        
        return Result<List<FriendRequest>>.Success(await _frRepo.GetFriendRequests(fr => fr.SenderId == senderId));
    }

    public async Task<Result<List<FriendRequest>>> GetReceivedFriendRequests(string recipientName) {
        var recipientId = await _accountRepository.GetAccountId(recipientName);
        if (recipientId == null) {
            return Result<List<FriendRequest>>.Failure("Recipient does not exist", false);
        }
        
        return Result<List<FriendRequest>>.Success(await _frRepo.GetFriendRequests(fr => fr.RecipientId == recipientId));
    }

    private async Task<Result<(string, string)>> GetIds(string senderName, string recipientName) {
        var senderId = await _accountRepository.GetAccountId(senderName);
        var recipientId = await _accountRepository.GetAccountId(recipientName);

        if (senderId == null) {
            return Result<(string, string)>.Failure("Sender does not exist", false);
        }
        
        if (recipientId == null) {
            return Result<(string, string)>.Failure("Recipient does not exist", false);
        }

        return Result<(string, string)>.Success((senderId, recipientId));
    }
}