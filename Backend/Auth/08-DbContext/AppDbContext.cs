using System.Collections.Immutable;
using Auth.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.DbContext;

public class AppDbContext: IdentityDbContext<User> {
    private ModelBuilder modelBuilder = null!;
    private readonly DeleteBehavior defaultDeleteBehavior = DeleteBehavior.Restrict;

    public AppDbContext(DbContextOptions options) : base(options) {}

    public DbSet<DocumentMetadata> DocumentsMetadata {get; set;}
    public DbSet<DocumentRole> DocumentRoles {get; set;}
    public DbSet<DocumentRoleGrant> DocumentRoleGrants {get; set;}
    public DbSet<UserDocumentRoleAssignment> UserDocumentRoleAssignments {get; set;}


    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        this.modelBuilder = modelBuilder;
        ConfigureDocumentMetadataModel();
        ConfigureDocumentRoleModel();
        ConfigureDocumentRoleGrantModel();
        ConfigureUserDocumentRoleAssignmentModel();
        SeedIdentityRoleModel();
        SeedDocumentRoleModel();
        this.modelBuilder = null!;
    }

    private void ConfigureDocumentMetadataModel() {
        modelBuilder.Entity<DocumentMetadata>(builder => {
            builder.HasKey(
                t => t.Id
            );
            builder
                .HasOne(t => t.Owner)
                .WithMany(u => u.CreatedTexts)
                .HasForeignKey(t => t.OwnerId)
                .OnDelete(defaultDeleteBehavior);
            builder
                .HasOne(t => t.DefaultRole)
                .WithMany()
                .HasForeignKey(t => t.DefaultRoleId)
                .OnDelete(defaultDeleteBehavior);
        });
    }

    private void ConfigureDocumentRoleModel() {
        modelBuilder.Entity<DocumentRole>(builder => {
            builder.HasKey(
                r => r.Name
            );
            builder.HasAlternateKey(
                r => new {r.CanRead, r.CanComment, r.CanEdit, 
                r.CanCreateRoleGrants, r.CanManageRoles}
            );
        });
    }

    private void ConfigureDocumentRoleGrantModel() {
        modelBuilder.Entity<DocumentRoleGrant>(builder => {
            builder.HasKey(
                g => g.Id
            );
            builder.HasAlternateKey(
                g => new {g.DocumentId, g.RoleId}
            );
            builder
                .HasOne(g => g.Document)
                .WithMany(t => t.RoleGrants)
                .HasForeignKey(g => g.DocumentId)
                .OnDelete(defaultDeleteBehavior);
            builder
                .HasOne(g => g.Role)
                .WithMany()
                .HasForeignKey(g => g.RoleId)
                .OnDelete(defaultDeleteBehavior);
        });
    }

    private void ConfigureUserDocumentRoleAssignmentModel() {
        modelBuilder.Entity<UserDocumentRoleAssignment>(builder => {
            builder.HasKey(
                r => new {r.UserId, r.TextId}
            );
            builder
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(defaultDeleteBehavior);
            builder
                .HasOne(r => r.Text)
                .WithMany()
                .HasForeignKey(r => r.TextId)
                .OnDelete(defaultDeleteBehavior);
            builder
                .HasOne(r => r.Role)
                .WithMany()
                .HasForeignKey(r => r.RoleId)
                .OnDelete(defaultDeleteBehavior);
        });
    }

    private void SeedIdentityRoleModel() {
        var PredefinedServiceRoles = ImmutableArray.Create(
            new IdentityRole() { Id = "4d1a8b3b-5d7d-4b5a-b7b3-3e4f3cbf54a8", Name = "Admin", 
                NormalizedName = "ADMIN", 
                ConcurrencyStamp = "8d6f4527-714e-4ed7-89fc-13cae731b39b" },
            new IdentityRole() { Id = "9f27a13e-4523-4b36-b9d1-981d356f0137", Name = "User", 
                NormalizedName = "USER", 
                ConcurrencyStamp = "3daa7139-2c08-4c90-8e32-e79861a4207f" }
        );

        modelBuilder.Entity<IdentityRole>().HasData(PredefinedServiceRoles);
    }

    private void SeedDocumentRoleModel() {
        var textRoles = new DocumentRolesPredefinedSet();

        modelBuilder.Entity<DocumentRole>().HasData(textRoles);
    }
}