using Microsoft.EntityFrameworkCore;

namespace MTPCoverageIssue.Database;

public class InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : DbContext(options)
{
    public DbSet<UpdateHistoryEntity> UpdateHistory { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UpdateHistoryConfiguration());
    }
}
