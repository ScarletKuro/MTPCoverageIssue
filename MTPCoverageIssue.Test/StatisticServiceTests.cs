using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MTPCoverageIssue.Database;
using MTPCoverageIssue.Mapping;

namespace MTPCoverageIssue.Test;

public class StatisticServiceTests
{
    [Fact]
    public async Task VersionTendencyWithingPeriodV3_WhenMemberRollsBack_UsesLatestEventNotMaxVersion()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var context = CreateInMemoryContext();
        context.UpdateHistory.AddRange(
            new UpdateHistoryEntity
            {
                ApplicationName = "app-1",
                MemberName = "member-a",
                FileName = "config.json",
                Version = new Version(3, 0),
                Status = "ok",
                UpdateDateTime = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc)
            },
            new UpdateHistoryEntity
            {
                ApplicationName = "app-1",
                MemberName = "member-a",
                FileName = "config.json",
                Version = new Version(2, 0),
                Status = "rollback",
                UpdateDateTime = new DateTime(2026, 1, 11, 0, 0, 0, DateTimeKind.Utc)
            },
            new UpdateHistoryEntity
            {
                ApplicationName = "app-1",
                MemberName = "member-b",
                FileName = "config.json",
                Version = new Version(2, 0),
                Status = "ok",
                UpdateDateTime = new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc)
            });
        await context.SaveChangesAsync(ct);

        var service = new StatisticService(context, CreateMapper());

        var result = service.VersionTendencyWithingPeriodV3("app-1", new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)).ToList();

        var v2 = Assert.Single(result);
        Assert.Equal(new Version(2, 0), v2.Version);
        Assert.Equal(2, v2.Count);
    }

    [Fact]
    public async Task VersionTendencyWithingPeriodV3_DoesNotLeakRowsFromOtherApplication()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var context = CreateInMemoryContext();
        context.UpdateHistory.AddRange(
            new UpdateHistoryEntity
            {
                ApplicationName = "app-1",
                MemberName = "member-a",
                FileName = "config.json",
                Version = new Version(2, 0),
                Status = "ok",
                UpdateDateTime = new DateTime(2026, 1, 11, 0, 0, 0, DateTimeKind.Utc)
            },
            new UpdateHistoryEntity
            {
                ApplicationName = "app-2",
                MemberName = "member-a",
                FileName = "config.json",
                Version = new Version(2, 0),
                Status = "ok",
                UpdateDateTime = new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc)
            });
        await context.SaveChangesAsync(ct);

        var service = new StatisticService(context, CreateMapper());

        var result = service.VersionTendencyWithingPeriodV3("app-1", new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)).ToList();

        var v2 = Assert.Single(result);
        Assert.Equal(new Version(2, 0), v2.Version);
        Assert.Equal(1, v2.Count);
        Assert.Equal("member-a", Assert.Single(v2.Memberships).MemberName);
    }

    private static InMemoryCentralConfigDbContext CreateInMemoryContext()
    {
        return new InMemoryCentralConfigDbContext(Guid.NewGuid().ToString("N"));
    }

    private static IMapper CreateMapper()
    {
        return new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
    }
}

public sealed class InMemoryCentralConfigDbContext : InMemoryDbContext
{
    private readonly string _databaseName;

    public InMemoryCentralConfigDbContext(string databaseName)
        : base(new DbContextOptions<InMemoryDbContext>())
    {
        _databaseName = databaseName;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseInMemoryDatabase(_databaseName);
    }
}