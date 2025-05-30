using System.Linq.Expressions;
using TextShareApi.ClassesLib;
using TextShareApi.Dtos.QueryOptions;
using TextShareApi.Exceptions;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;
using TextShareApi.Models;

namespace TextShareApi.Services;

public class FriendRequestService : IFriendRequestService {
    private readonly IAccountRepository _accountRepository;
    private readonly IFriendPairRepository _friendPairRepository;
    private readonly IFriendRequestRepository _friendRequestRepository;
    private readonly ILogger<FriendRequestService> _logger;

    public FriendRequestService(IFriendRequestRepository friendRequestRepository, IAccountRepository accountRepository,
        IFriendPairRepository friendPairRepository,
        ILogger<FriendRequestService> logger) {
        _friendRequestRepository = friendRequestRepository;
        _accountRepository = accountRepository;
        _friendPairRepository = friendPairRepository;
        _logger = logger;
    }

    public async Task<Result<FriendRequest>> Create(string senderName, string recipientName) {
        if (senderName == recipientName)
            return Result<FriendRequest>.Failure(
                new BadRequestException("Sender name and recipient name cannot be the same."));

        var (senderId, recipientId) = await _accountRepository.GetAccountIds(senderName, recipientName);
        if (senderId == null) return Result<FriendRequest>.Failure(new NotFoundException("Sender not found."));
        if (recipientId == null) return Result<FriendRequest>.Failure(new NotFoundException("Recipient not found."));


        var areFriends = await _friendPairRepository.ContainsFriendPair(senderId, recipientId);
        if (areFriends) return Result<FriendRequest>.Failure(new BadRequestException("Users are friends already."));
        var exists = await _friendRequestRepository.ContainsRequest(senderId, recipientId);
        if (exists) return Result<FriendRequest>.Failure(new BadRequestException("Request already exists."));
        exists = await _friendRequestRepository.ContainsRequest(recipientId, senderId);
        if (exists) return Result<FriendRequest>.Failure(new BadRequestException("Reverse request already exists."));

        var request = await _friendRequestRepository.CreateRequest(senderId, recipientId);

        var sender = new AppUser { Id = senderId, UserName = senderName };
        var recipient = new AppUser { Id = recipientId, UserName = recipientName };
        request.Sender = sender;
        request.Recipient = recipient;

        return Result<FriendRequest>.Success(request);
    }

    public async Task<Result> Delete(string senderName, string recipientName) {
        if (senderName == recipientName)
            return Result.Failure(new BadRequestException("Sender name and recipient name cannot be the same."));

        var (senderId, recipientId) = await _accountRepository.GetAccountIds(senderName, recipientName);
        if (senderId == null) return Result.Failure(new NotFoundException("Sender not found."));
        if (recipientId == null) return Result.Failure(new NotFoundException("Recipient not found."));

        var isDeleted = await _friendRequestRepository.DeleteRequest(senderId, recipientId);
        if (!isDeleted) return Result.Failure(new BadRequestException("Did not exist from the beginning."));
        return Result.Success();
    }

    public async Task<Result<FriendRequest>> Process(string senderName, string recipientName, bool acceptRequest) {
        if (senderName == recipientName)
            return Result<FriendRequest>.Failure(
                new BadRequestException("Sender name and recipient name cannot be the same."));

        var (senderId, recipientId) = await _accountRepository.GetAccountIds(senderName, recipientName);
        if (senderId == null) return Result<FriendRequest>.Failure(new NotFoundException("Sender not found."));
        if (recipientId == null) return Result<FriendRequest>.Failure(new NotFoundException("Recipient not found."));

        var request = await _friendRequestRepository.GetRequest(senderId, recipientId);
        if (request == null) return Result<FriendRequest>.Failure(new NotFoundException("Request not found."));

        if (acceptRequest) {
            await _friendPairRepository.CreateFriendPairs(senderId, recipientId);
            var deleted = await _friendRequestRepository.DeleteRequest(senderId, recipientId);
            if (!deleted) _logger.LogError($"Failed to delete processed request from {senderName} to {recipientName}.");
            request.IsAccepted = true;
            return Result<FriendRequest>.Success(request);
        }

        request = await _friendRequestRepository.UpdateRequest(senderId, recipientId, false);
        if (request is null) return Result<FriendRequest>.Failure(new ServerException());
        return Result<FriendRequest>.Success(request);
    }

    public async Task<Result<FriendRequest>> GetFriendRequest(string senderName, string recipientName) {
        if (senderName == recipientName)
            return Result<FriendRequest>.Failure(
                new BadRequestException("Sender name and recipient name cannot be the same."));

        var (senderId, recipientId) = await _accountRepository.GetAccountIds(senderName, recipientName);
        if (senderId == null) return Result<FriendRequest>.Failure(new NotFoundException("Sender not found."));
        if (recipientId == null) return Result<FriendRequest>.Failure(new NotFoundException("Recipient not found."));

        var request = await _friendRequestRepository.GetRequest(senderId, recipientId);
        if (request == null) return Result<FriendRequest>.Failure(new NotFoundException("Request not found."));

        return Result<FriendRequest>.Success(request);
    }

    public async Task<Result<PaginatedResponseDto<FriendRequest>>> GetSentFriendRequests(PaginationDto pagination,
        bool isAscending,
        string senderName,
        string? recipientName) {
        var senderId = await _accountRepository.GetAccountId(senderName);
        if (senderId == null)
            return Result<PaginatedResponseDto<FriendRequest>>.Failure(new ServerException("Sender not found."));

        var skip = (pagination.PageNumber - 1) * pagination.PageSize;
        var take = pagination.PageSize;

        Expression<Func<FriendRequest, string>> orderBy = p => p.Recipient.UserName!;

        var predicates = new List<Expression<Func<FriendRequest, bool>>> {
            p => p.SenderId == senderId
        };
        if (recipientName != null && recipientName.Trim() != "")
            predicates.Add(p => p.Recipient.UserName!.ToLower().Contains(recipientName.ToLower()));

        var (count, pairs) = await _friendRequestRepository.GetFriendRequests(
            skip,
            take,
            orderBy,
            isAscending,
            predicates
        );

        return Result<PaginatedResponseDto<FriendRequest>>.Success(pairs.ToPaginatedResponse(pagination, count));
    }

    public async Task<Result<PaginatedResponseDto<FriendRequest>>> GetReceivedFriendRequests(PaginationDto pagination,
        bool isAscending,
        string? senderName,
        string recipientName) {
        var recipientId = await _accountRepository.GetAccountId(recipientName);
        if (recipientId == null)
            return Result<PaginatedResponseDto<FriendRequest>>.Failure(new ServerException("Recipient not found."));

        var skip = (pagination.PageNumber - 1) * pagination.PageSize;
        var take = pagination.PageSize;

        Expression<Func<FriendRequest, string>> orderBy = p => p.Sender.UserName!;

        var predicates = new List<Expression<Func<FriendRequest, bool>>> {
            p => p.RecipientId == recipientId
        };
        if (senderName != null && senderName.Trim() != "")
            predicates.Add(p => p.Sender.UserName!.ToLower().Contains(senderName.ToLower()));

        var (count, pairs) = await _friendRequestRepository.GetFriendRequests(
            skip,
            take,
            orderBy,
            isAscending,
            predicates
        );

        return Result<PaginatedResponseDto<FriendRequest>>.Success(pairs.ToPaginatedResponse(pagination, count));
    }
}