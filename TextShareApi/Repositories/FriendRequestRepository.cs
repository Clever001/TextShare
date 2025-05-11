using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Models;

namespace TextShareApi.Repositories;

public class FriendRequestRepository : IFriendRequestRepository {
    private readonly AppDbContext _context;

    public FriendRequestRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<FriendRequest> CreateRequest(string senderId, string recipientId) {
        var request = new FriendRequest {
            SenderId = senderId,
            RecipientId = recipientId
        };

        await _context.FriendRequests.AddAsync(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<FriendRequest?> GetRequest(string senderId, string recipientId) {
        return await _context.FriendRequests
            .Include(r => r.Sender)
            .Include(r => r.Recipient)
            .FirstOrDefaultAsync(r => r.SenderId == senderId && r.RecipientId == recipientId);
    }

    public async Task<(int, List<FriendRequest>)> GetFriendRequests<T>(int skip,
        int take,
        Expression<Func<FriendRequest, T>> keyOrder,
        bool isAscending,
        List<Expression<Func<FriendRequest, bool>>>? predicates) 
    {
        IQueryable<FriendRequest> requests = _context.FriendRequests
            .Include(r => r.Sender)
            .Include(r => r.Recipient);
        
        // Filtering
        if (predicates != null) {
            foreach (var predicate in predicates) {
                requests = requests.Where(predicate);
            }
        }
        
        int count = await requests.CountAsync();
        
        // Ordering
        requests = isAscending ? requests.OrderBy(keyOrder) : requests.OrderByDescending(keyOrder);
        
        // Pagination
        requests = requests.Skip(skip).Take(take);
        
        return (count, await requests.ToListAsync());
    }

    public async Task<FriendRequest?> UpdateRequest(string senderId, string recipientId, bool isAccepted) {
        var request = await _context.FriendRequests.FindAsync(senderId, recipientId);
        if (request is null) return null;

        request.IsAccepted = isAccepted;
        _context.FriendRequests.Update(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<bool> DeleteRequest(string senderId, string recipientId) {
        var request = await _context.FriendRequests.FindAsync(senderId, recipientId);
        if (request is null) return false;

        _context.Remove(request);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ContainsRequest(string senderId, string recipientId) {
        return await _context.FriendRequests.AnyAsync(r => r.SenderId == senderId && r.RecipientId == recipientId);
    }
}