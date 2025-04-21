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
            return Result<FriendRequest>.Failure("Already exists");
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
            return Result.Failure("Did not exist from the beginning");
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
            return Result<FriendRequest>.Failure("Does not exist");
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
}