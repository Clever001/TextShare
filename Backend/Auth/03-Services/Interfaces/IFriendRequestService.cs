using Auth.Models;
using Auth.Other;
using Shared;


namespace Auth.Services.Interfaces;

public interface IFriendRequestService {
    Task<Result<FriendRequest>> Create(string senderName, string recipientName);
    Task<Result> Delete(string senderName, string recipientName);
    Task<Result<FriendRequest>> Process(string senderName, string recipientName, bool acceptRequest);
    Task<Result<FriendRequest>> GetFriendRequest(string senderName, string recipientName);

    Task<Result<PaginatedResponse<FriendRequest>>> GetSentFriendRequests(PaginationCommand pagination, bool isAscending,
        string senderName, string? recipientName);

    Task<Result<PaginatedResponse<FriendRequest>>> GetReceivedFriendRequests(PaginationCommand pagination,
        bool isAscending,
        string? senderName, string recipientName);
}