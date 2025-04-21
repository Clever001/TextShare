using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Interfaces;
using TextShareApi.Models;

namespace TextShareApi.Repositories;

public class FriendRequestRepository : IFriendRequestRepository {
    private readonly AppDbContext _context;

    public FriendRequestRepository(AppDbContext context) {
        _context = context;
    }
    
    public async Task<FriendRequest> CreateRequest(string userId, string recipientId) {
        var request = new FriendRequest {SenderId = userId, RecipientId = recipientId};
        await _context.FriendRequests.AddAsync(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<FriendRequest?> GetRequest(string userId, string recipientId) {
        return await _context.FriendRequests
            .Include(r => r.Sender.UserName)
            .Include(r => r.Recipient.UserName)
            .FirstOrDefaultAsync(r => r.SenderId == userId && r.RecipientId == recipientId);
    }
    
    public async Task<FriendRequest?> UpdateRequest(string userId, string recipientId, bool isAccepted) {
        var request = await GetRequest(userId, recipientId);
        if (request is null) {
            return null;
        }
        
        request.IsAccepted = isAccepted;
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<bool> DeleteRequest(string userId, string recipientId) {
        var request = await GetRequest(userId, recipientId);
        if (request is null) {
            return false;
        }

        _context.Remove(request);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ContainsRequest(string userId, string recipientId) {
        return await _context.FriendRequests.AnyAsync(r => r.SenderId == userId && r.RecipientId == recipientId);
    }
}