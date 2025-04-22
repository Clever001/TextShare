using Microsoft.AspNetCore.Identity;
using TextShareApi.Dtos.FriendRequest;
using TextShareApi.Interfaces;
using TextShareApi.Mappers;
using TextShareApi.Models;

namespace TextShareApi.Services;

public class FriendRequestService : IFriendRequestService {
    private readonly IFriendRequestRepository _frRepo;
    private readonly UserManager<AppUser> _userManager;

    public FriendRequestService(IFriendRequestRepository frRepo, UserManager<AppUser> userManager) {
        _frRepo = frRepo;
        _userManager = userManager;
    }
    
    public async Task<Result<FriendRequest>> Create(string senderName, string recipientName) {
        var idResult = await GetId(senderName, recipientName);
        if (!idResult.IsSuccess) {
            return Result<FriendRequest>.Failure(idResult.Error);
        }
        var (senderId, recipientId) = idResult.Value;

        bool exists = await _frRepo.ContainsRequest(senderId, recipientId);
        if (exists) {
            return Result<FriendRequest>.Failure("Already exists", true);
        }
        
        var request = await _frRepo.CreateRequest(senderId, recipientId);
        return Result<FriendRequest>.Success(request);
    }

    public async Task<Result> Delete(string senderName, string recipientName) {
        var idResult = await GetId(senderName, recipientName);
        if (!idResult.IsSuccess) {
            return Result.Failure(idResult.Error);
        }
        var (senderId, recipientId) = idResult.Value;
        
        bool isDeleted = await _frRepo.DeleteRequest(senderId, recipientId);
        if (!isDeleted) {
            return Result.Failure("Did not exist from the beginning", true);
        }
        return Result.Success();
    }

    public async Task<Result<FriendRequest>> Process(string senderName, string recipientName, bool acceptRequest) {
        var idResult = await GetId(senderName, recipientName);
        if (!idResult.IsSuccess) {
            return Result<FriendRequest>.Failure(idResult.Error);
        }
        var (senderId, recipientId) = idResult.Value;
        
        var request = await _frRepo.UpdateRequest(senderId, recipientId, acceptRequest);
        if (request is null) {
            return Result<FriendRequest>.Failure("Does not exist", true);
        }

        return Result<FriendRequest>.Success(request);
    }

    public async Task<Result<FriendRequest?>> GetFriendRequest(string senderName, string recipientName) {
        var idResult = await GetId(senderName, recipientName);
        if (!idResult.IsSuccess) {
            return Result<FriendRequest?>.Failure(idResult.Error);
        }
        var (senderId, recipientId) = idResult.Value;
        
        var request = await _frRepo.GetRequest(senderId, recipientId);
        return Result<FriendRequest?>.Success(request);
    }

    public async Task<Result<List<FriendRequest>>> GetSentFriendRequests(string senderName) {
        var idResult = await GetId(senderName);
        if (!idResult.IsSuccess) {
            return Result<List<FriendRequest>>.Failure(idResult.Error);
        }
        string senderId = idResult.Value;
        
        return Result<List<FriendRequest>>.Success(await _frRepo.GetFriendRequests(fr => fr.SenderId == senderId));
    }

    public async Task<Result<List<FriendRequest>>> GetReceivedFriendRequests(string recipientName) {
        var idResult = await GetId(recipientName);
        if (!idResult.IsSuccess) {
            return Result<List<FriendRequest>>.Failure(idResult.Error);
        }
        string recipientId = idResult.Value;
        
        return Result<List<FriendRequest>>.Success(await _frRepo.GetFriendRequests(fr => fr.RecipientId == recipientId));
    }

    private async Task<Result<(string, string)>> GetId(string userName, string recipientName) {
        var sender = await _userManager.FindByNameAsync(userName);
        var recipient = await _userManager.FindByNameAsync(recipientName);

        if (sender == null) {
            return Result<(string, string)>.Failure("Sender does not exist");
        }
        
        if (recipient == null) {
            return Result<(string, string)>.Failure("Recipient does not exist");
        }

        return Result<(string, string)>.Success((sender.Id, recipient.Id));
    }

    private async Task<Result<string>> GetId(string userName) {
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null) {
            return Result<string>.Failure("User does not exist");
        }

        return Result<string>.Success(user.Id);
    }
}