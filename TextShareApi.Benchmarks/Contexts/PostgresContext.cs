using Microsoft.EntityFrameworkCore;
using TextShareApi.Benchmarks.Models;

namespace TextShareApi.Benchmarks.Contexts;

public class PostgresContext : DbContext {
    public PostgresContext(DbContextOptions options) : base(options) { }

    public DbSet<Text> Texts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        builder.Entity<Text>().HasKey(x => x.Id);
    }
}