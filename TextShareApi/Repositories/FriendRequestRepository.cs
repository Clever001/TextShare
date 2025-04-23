using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Models;

namespace TextShareApi.Repositories;

public class FriendRequestRepository : IFriendRequestRepository {
    private readonly AppDbContext _context;
    private readonly IAccountRepository _accountRepository;

    public FriendRequestRepository(AppDbContext context, IAccountRepository accountRepository) {
        _context = context;
        _accountRepository = accountRepository;
    }
    
    public async Task<FriendRequest> CreateRequest(string senderId, string recipientId) {
        string? senderName = await _accountRepository.GetUserName(senderId);
        string? recipientName = await _accountRepository.GetUserName(recipientId);
        
        Debug.Assert(senderName != null, nameof(senderName) + " != null");
        Debug.Assert(recipientName != null, nameof(recipientName) + " != null");

        var sender = new AppUser { Id = senderId, UserName = senderName };
        var recipient = new AppUser { Id = recipientId, UserName = recipientName };
        
        var request = new FriendRequest {
            SenderId = senderId, 
            RecipientId = recipientId,
            Sender = sender,
            Recipient = recipient,
        };
        await _context.FriendRequests.AddAsync(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<FriendRequest?> GetRequest(string senderId, string recipientId) {
        return await _context.FriendRequests
            .Include(r => r.Sender)
                .ThenInclude(u => u.UserName)
            .Include(r => r.Recipient)
                .ThenInclude(u => u.UserName)
            .FirstOrDefaultAsync(r => r.SenderId == senderId && r.RecipientId == recipientId);
    }

    public async Task<List<FriendRequest>> GetFriendRequests(Expression<Func<FriendRequest, bool>> predicate) {
        return await _context.FriendRequests
            .Include(r => r.Sender)
                .ThenInclude(u => u.UserName)
            .Include(r => r.Recipient)
                .ThenInclude(u => u.UserName)
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<FriendRequest?> UpdateRequest(string senderId, string recipientId, bool isAccepted) {
        var request = await GetRequest(senderId, recipientId);
        if (request is null) {
            return null;
        }
        
        request.IsAccepted = isAccepted;
        _context.FriendRequests.Update(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<bool> DeleteRequest(string senderId, string recipientId) {
        var request = await GetRequest(senderId, recipientId);
        if (request is null) {
            return false;
        }

        _context.Remove(request);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ContainsRequest(string senderId, string recipientId) {
        return await _context.FriendRequests.AnyAsync(r => r.SenderId == senderId && r.RecipientId == recipientId);
    }
}