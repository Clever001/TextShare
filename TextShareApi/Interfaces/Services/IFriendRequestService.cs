using TextShareApi.ClassesLib;
using TextShareApi.Dtos.QueryOptions;
using TextShareApi.Models;

namespace TextShareApi.Interfaces.Services;

public interface IFriendRequestService {
    Task<Result<FriendRequest>> Create(string senderName, string recipientName);
    Task<Result> Delete(string senderName, string recipientName);
    Task<Result<FriendRequest>> Process(string senderName, string recipientName, bool acceptRequest);
    Task<Result<FriendRequest?>> GetFriendRequest(string senderName, string recipientName);
    Task<Result<List<FriendRequest>>> GetSentFriendRequests(PaginationDto pagination, bool isAscending,
        string senderName, string? recipientName);
    Task<Result<List<FriendRequest>>> GetReceivedFriendRequests(PaginationDto pagination, bool isAscending,
        string? senderName, string recipientName);
}