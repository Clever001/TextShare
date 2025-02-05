using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Models;

namespace TextShareApi.Data;

public class AppDbContext : IdentityDbContext<AppUser> {
    
    public AppDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<Message> Messages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        var roles = new List<IdentityRole>
        {
            new IdentityRole { Id = "4d1a8b3b-5d7d-4b5a-b7b3-3e4f3cbf54a8", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "9f27a13e-4523-4b36-b9d1-981d356f0137", Name = "User", NormalizedName = "USER" }
        };
        builder.Entity<IdentityRole>().HasData(roles);
        
        var messages = new List<Message>
        {
            new Message { Id = 1, Title = "First Message", Content = "Hello World!" },
            new Message { Id = 2, Title = "Second Message", Content = "Does it work?!" }
        };
        builder.Entity<Message>().HasData(messages);
    }
}