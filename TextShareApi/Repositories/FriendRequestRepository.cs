using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Interfaces;
using TextShareApi.Models;

namespace TextShareApi.Repositories;

public class FriendRequestRepository : IFriendRequestRepository {
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public FriendRequestRepository(AppDbContext context, UserManager<AppUser> userManager) {
        _context = context;
        _userManager = userManager;
    }
    
    public async Task<FriendRequest> CreateRequest(string senderId, string recipientId) {
        // TODO: Может здесь стоит использовать репозиторий пользователей?
        var senderData = await _context.Users
            .Select(u => new {u.Id, u.UserName})
            .FirstOrDefaultAsync(u => u.Id == senderId);
        var recipientData = await _context.Users
            .Select(u => new {u.Id, u.UserName})
            .FirstOrDefaultAsync(u => u.Id == recipientId);

        var sender = new AppUser { Id = senderData!.Id, UserName = senderData.UserName };
        var recipient = new AppUser { Id = recipientData!.Id, UserName = recipientData.UserName };
        
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