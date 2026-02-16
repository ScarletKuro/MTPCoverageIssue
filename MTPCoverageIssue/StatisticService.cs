using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using MTPCoverageIssue.Database;
using MTPCoverageIssue.Extensions;
using MTPCoverageIssue.Model;

namespace MTPCoverageIssue;

public class StatisticService
{
    private readonly InMemoryDbContext _dbContext;
    private readonly IMapper _mapper;

    public StatisticService(InMemoryDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [ExcludeFromCodeCoverage]
    [Obsolete($"Use {nameof(VersionTendencyWithingPeriodV3)}")]
    public IEnumerable<VersionCountDto> VersionTendencyWithingPeriod(string applicationName, DateTime dateTimePeriodFrom)
    {
        var versionHistory = _dbContext
            .UpdateHistory
            .Where(history => history.ApplicationName == applicationName)
            .UpdateHistoryFrom(dateTimePeriodFrom)
            .ToList()
            .GroupBy(history => history.ApplicationName)
            .Select(historyGroup => new HistoryGroupByApplicationName(
                historyGroup.Key,
                historyGroup
                    .OrderByDescending(history => history.Version)
                    .DistinctBy(history => history.MemberName)
            ))
            .SelectMany(historyGroupByApplicationName => historyGroupByApplicationName.History)
            .GroupBy(history => history.Version)
            .Select(x =>
                new CountGroupByVersion(
                    x.Key,
                    x.Count(),
                    x.Select(member => new CountGroupByVersion.Membership(member.Version, member.MemberName))));

        return _mapper.Map<IEnumerable<VersionCountDto>>(versionHistory);
    }

    public IEnumerable<VersionCountDto> VersionTendencyWithingPeriodV3(string applicationName, DateTime dateTimePeriodFrom)
    {
        var filteredHistory = _dbContext.UpdateHistory
            .Where(uh => uh.ApplicationName == applicationName && uh.UpdateDateTime >= dateTimePeriodFrom);

        var maxUpdateDatePerMember = filteredHistory
            .GroupBy(uh => uh.MemberName)
            .Select(g => new { MemberName = g.Key, MaxUpdateDateTime = g.Max(x => x.UpdateDateTime) });

        var latestRowsByDate = filteredHistory
            .Join(maxUpdateDatePerMember,
                history => new { history.MemberName, history.UpdateDateTime },
                maxDate => new { maxDate.MemberName, UpdateDateTime = maxDate.MaxUpdateDateTime },
                (history, _) => history);

        // If multiple rows exist at the same timestamp for a member, keep the latest inserted row.
        var maxIdPerMember = latestRowsByDate
            .GroupBy(uh => uh.MemberName)
            .Select(g => new { MemberName = g.Key, MaxId = g.Max(x => x.Id) });

        var latestPerMember = latestRowsByDate
            .Join(maxIdPerMember,
                history => new { history.MemberName, history.Id },
                maxId => new { maxId.MemberName, Id = maxId.MaxId },
                (history, _) => history);

        //Convert to old format
        var versionHistory = latestPerMember
            .ToList()
            .GroupBy(x=> x.Version)
            .Select(x=> new CountGroupByVersion(x.Key, x.Count(), x.Select(member => new CountGroupByVersion.Membership(member.Version, member.MemberName))))
            .ToList();


        return _mapper.Map<IEnumerable<VersionCountDto>>(versionHistory);
    }
}
