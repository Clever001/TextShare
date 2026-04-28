using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DocShareApi.Models;

namespace DocShareApi.Data;

public class AppDbContext : IdentityDbContext<AppUser> {
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Comment> Comments { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<DocVersion> DocVerions { get; set; }
    public DbSet<HashSeed> HashSeeds { get; set; }
    public DbSet<PublishedVersion> PublishedVersion { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<UserToDocRole> UserToDocRoles { get; set; }
    public DbSet<DocToTag> DocToTags {get;set;}


    private static readonly IdentityRole[] SeedRoles = {
        new() { Id = "4d1a8b3b-5d7d-4b5a-b7b3-3e4f3cbf54a8", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "8d6f4527-714e-4ed7-89fc-13cae731b39b" },
        new() { Id = "9f27a13e-4523-4b36-b9d1-981d356f0137", Name = "User", NormalizedName = "USER", ConcurrencyStamp = "3daa7139-2c08-4c90-8e32-e79861a4207f" }
    };

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        builder.Entity<IdentityRole>().HasData(SeedRoles);
        builder.Entity<Document>(e => {
            e.HasKey(d => d.Id);
            e.HasOne(d => d.Owner)
                .WithMany(u => u.Documents)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(d => new { d.Title, d.OwnerId }).IsUnique();
        });
        builder.Entity<Tag>(e => {
            e.HasKey(t => t.Name);
            e.HasMany(t => t.Documents)
                .WithMany(d => d.Tags)
                .UsingEntity<DocToTag>(
                    l => l.HasOne(dt => dt.Document)
                        .WithMany()
                        .HasForeignKey(dt => dt.DocumentId)
                        .OnDelete(DeleteBehavior.Restrict),
                    r => r.HasOne(dt => dt.Tag)
                        .WithMany()
                        .HasForeignKey(dt => dt.TagName)
                        .OnDelete(DeleteBehavior.Restrict),
                    j => {
                        j.ToTable("DocToTag");
                        j.HasKey(dt => new {dt.DocumentId, dt.TagName});
                    }
                );
        });
        builder.Entity<UserToDocRole>(e => {
            e.HasKey(r => new { r.UserId, r.DocumentId });
            e.HasOne(r => r.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(r => r.Document)
                .WithMany(d => d.UserRoles)
                .HasForeignKey(r => r.DocumentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        builder.Entity<DocVersion>(e => {
            e.HasKey(v => v.Id);
            e.HasOne(v => v.Document)
                .WithMany(d => d.Versions)
                .HasForeignKey(v => v.DocumentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        builder.Entity<PublishedVersion>(e => {
            e.HasKey(v => v.DocumentId);
            e.HasOne(v => v.Document)
                .WithOne(d => d.PublishedVersion)
                .HasForeignKey<PublishedVersion>(v => v.DocumentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        builder.Entity<Comment>(e => {
            e.HasKey(c => c.Id);
            e.HasOne(c => c.Parent)
                .WithMany()
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(c => c.Document)
                .WithMany(d => d.Comments)
                .HasForeignKey(c => c.DocumentId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
