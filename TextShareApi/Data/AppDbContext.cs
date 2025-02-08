using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Models;

namespace TextShareApi.Data;

public class AppDbContext : IdentityDbContext<AppUser> {
    
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Text> Texts { get; set; }
    public DbSet<HashSeed> HashSeeds { get; set; }
    public DbSet<FriendRequest> FriendRequests { get; set; }
    public DbSet<FriendPair> FriendPairs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        var roles = new List<IdentityRole>
        {
            new IdentityRole { Id = "4d1a8b3b-5d7d-4b5a-b7b3-3e4f3cbf54a8", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "9f27a13e-4523-4b36-b9d1-981d356f0137", Name = "User", NormalizedName = "USER" }
        };
        builder.Entity<IdentityRole>().HasData(roles);

        builder.Entity<Text>().HasKey(t => t.Id);
        builder.Entity<Text>()
            .HasOne(t => t.AppUser)
            .WithMany(u => u.Texts)
            .HasForeignKey(t => t.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Text>().HasIndex(t => t.AppUserId);

        builder.Entity<FriendRequest>().HasKey(r => new { r.SenderId, r.RecipientId });
        builder.Entity<FriendRequest>()
            .HasOne(r => r.Sender)
            .WithMany(u => u.FriendRequests)
            .HasForeignKey(r => r.SenderId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<FriendRequest>()
            .HasOne(r => r.Recipient)
            .WithMany()
            .HasForeignKey(r => r.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<FriendRequest>()
            .HasIndex(r => r.SenderId);
        builder.Entity<FriendRequest>()
            .HasIndex(r => r.RecipientId);
        
        builder.Entity<FriendPair>().HasKey(f => new { f.FirstUserId, f.SecondUserId });
        builder.Entity<FriendPair>()
            .HasOne(p => p.FirstUser)
            .WithMany(u => u.FriendPairs)
            .HasForeignKey(p => p.FirstUserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<FriendPair>()
            .HasOne(p => p.SecondUser)
            .WithMany()
            .HasForeignKey(p => p.SecondUserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<FriendPair>()
            .HasIndex(p => p.FirstUserId);
        builder.Entity<FriendPair>()
            .HasIndex(p => p.SecondUserId);
    }
}