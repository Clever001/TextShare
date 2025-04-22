using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Interfaces;
using TextShareApi.Models;

namespace TextShareApi.Repositories;

public class FriendPairsRepository : IFriendPairsRepository {
    private readonly AppDbContext _context;

    public FriendPairsRepository(AppDbContext context) {
        _context = context;
    }
    
    public async Task<List<string>> GetFriendsIds(string userId) {
        var friends = await _context.FriendPairs
            .Where(p => p.FirstUserId == userId)
            .Select(p => p.SecondUserId)
            .ToListAsync();
        return friends;
    }
}