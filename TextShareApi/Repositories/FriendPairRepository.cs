using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Models;

namespace TextShareApi.Repositories;

public class FriendPairRepository : IFriendPairRepository {
    private readonly AppDbContext _context;

    public FriendPairRepository(AppDbContext context) {
        _context = context;
    }
    
    public async Task<List<string>> GetFriendsIds(string userId) {
        // TODO: Replace to Service.
        var friends = await _context.FriendPairs
            .Where(p => p.FirstUserId == userId)
            .Select(p => p.SecondUserId)
            .ToListAsync();
        return friends;
    }

    public async Task<(FriendPair, FriendPair)> CreateFriendPairs(string firstUserId, string secondUserId) {
        var firstUserData = _context.Users
            .Select(u => new {u.Id, u.UserName})
            .FirstOrDefault(u => u.Id == firstUserId);
        var secondUserData = _context.Users
            .Select(u => new {u.Id, u.UserName})
            .FirstOrDefault(u => u.Id == secondUserId);

        var firstUser = new AppUser { Id = firstUserData!.Id, UserName = firstUserData.UserName };
        var secondUser = new AppUser { Id = secondUserData!.Id, UserName = secondUserData.UserName };
        
        var firstPair = new FriendPair {
            FirstUserId = firstUserId, 
            SecondUserId = secondUserId,
            FirstUser = firstUser,
            SecondUser = secondUser
        };
        var secondPair = new FriendPair {
            FirstUserId = secondUserId, 
            SecondUserId = firstUserId,
            FirstUser = secondUser,
            SecondUser = firstUser
        };
        
        await _context.FriendPairs.AddAsync(firstPair);
        await _context.FriendPairs.AddAsync(secondPair);
        await _context.SaveChangesAsync();
        return (firstPair, secondPair);
    }

    public async Task<FriendPair?> GetFriendPair(string firstUserId, string secondUserId) {
        return await _context.FriendPairs
            .Include(p => p.SecondUser)
                .ThenInclude(u => u.UserName)
            .FirstOrDefaultAsync(p => p.FirstUserId == firstUserId && p.SecondUserId == secondUserId);
    }

    public async Task<List<FriendPair>> GetFriendPairs(Expression<Func<FriendPair, bool>> predicate) {
        return await _context.FriendPairs
            .Include(p => p.SecondUser)
                .ThenInclude(u => u.UserName)
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<bool> DeleteFriendPairs(string firstUserId, string secondUserId) {
        var firstPair = await _context.FriendPairs
            .FirstOrDefaultAsync(p => p.FirstUserId == firstUserId && p.SecondUserId == secondUserId);
        var secondPair = await _context.FriendPairs
            .FirstOrDefaultAsync(p => p.FirstUserId == secondUserId && p.SecondUserId == firstUserId);

        if (firstPair == null || secondPair == null) {
            return false;
        }
        
        _context.FriendPairs.Remove(firstPair);
        _context.FriendPairs.Remove(secondPair);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ContainsFriendPair(string firstUserId, string secondUserId) {
        return await _context.FriendPairs
            .AnyAsync(p => p.FirstUserId == firstUserId && p.SecondUserId == secondUserId);
    }
}