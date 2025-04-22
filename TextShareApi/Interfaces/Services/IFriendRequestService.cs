using TextShareApi.Dtos.FriendRequest;
using TextShareApi.Models;

namespace TextShareApi.Interfaces;

public interface IFriendRequestService {
    Task<Result<FriendRequest>> Create(string senderName, string recipientName);
    Task<Result> Delete(string senderName, string recipientName);
    Task<Result<FriendRequest>> Process(string senderName, string recipientName, bool acceptRequest);
    Task<Result<FriendRequest?>> GetFriendRequest(string senderName, string recipientName);
    Task<Result<List<FriendRequest>>> GetSentFriendRequests(string senderName);
    Task<Result<List<FriendRequest>>> GetReceivedFriendRequests(string recipientName);
}