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
        var idResult = await GetIds(senderName, recipientName);
        if (!idResult.IsSuccess) {
            return Result<FriendRequest>.Failure(idResult.Error, false);
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
        var idResult = await GetIds(senderName, recipientName);
        if (!idResult.IsSuccess) {
            return Result<FriendRequest>.Failure(idResult.Error, false);
        }
        var (senderId, recipientId) = idResult.Value;
        
        var request = await _frRepo.UpdateRequest(senderId, recipientId, acceptRequest);
        if (request is null) {
            return Result<FriendRequest>.Failure("Does not exist", true);
        }
        // TODO: Разобраться с изменениями в запросах в друзья и в парах друзей при добавлении и удалении пользователей из списка друзей.
        // Add To Friend List
        var result = await _friendService.AddFriend(senderId, recipientId);
        if (!result.IsSuccess) {
            return Result<FriendRequest>.Failure($"Error while adding recipient to friends list: {result.Error}", false);
        }

        return Result<FriendRequest>.Success(request);
    }

    public async Task<Result<FriendRequest?>> GetFriendRequest(string senderName, string recipientName) {
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