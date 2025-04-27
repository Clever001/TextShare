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
        var friends = await _context.FriendPairs
            .Where(p => p.FirstUserId == userId)
            .Select(p => p.SecondUserId)
            .ToListAsync();
        return friends;
    }

    public async Task<(FriendPair, FriendPair)> CreateFriendPairs(string firstUserId, string secondUserId) {
        var firstPair = new FriendPair {
            FirstUserId = firstUserId,
            SecondUserId = secondUserId
        };
        var secondPair = new FriendPair {
            FirstUserId = secondUserId,
            SecondUserId = firstUserId
        };

        await _context.FriendPairs.AddAsync(firstPair);
        await _context.FriendPairs.AddAsync(secondPair);
        await _context.SaveChangesAsync();

        return (firstPair, secondPair);
    }

    public async Task<FriendPair?> GetFriendPair(string firstUserId, string secondUserId) {
        return await _context.FriendPairs
            .Include(p => p.SecondUser)
            .FirstOrDefaultAsync(p => p.FirstUserId == firstUserId && p.SecondUserId == secondUserId);
    }

    public async Task<List<FriendPair>> GetFriendPairs(Expression<Func<FriendPair, bool>> predicate) {
        return await _context.FriendPairs
            .Include(p => p.SecondUser)
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<bool> DeleteFriendPairs(string firstUserId, string secondUserId) {
        var firstPair = await _context.FriendPairs
            .FirstOrDefaultAsync(p => p.FirstUserId == firstUserId && p.SecondUserId == secondUserId);
        var secondPair = await _context.FriendPairs
            .FirstOrDefaultAsync(p => p.FirstUserId == secondUserId && p.SecondUserId == firstUserId);

        if (firstPair == null || secondPair == null) return false;

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