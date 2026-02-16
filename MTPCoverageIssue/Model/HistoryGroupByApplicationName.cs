using MTPCoverageIssue.Database;

namespace MTPCoverageIssue.Model;

public class HistoryGroupByApplicationName(string applicationName, IEnumerable<UpdateHistoryEntity> history)
{
    public string ApplicationName { get; } = applicationName;

    public IEnumerable<UpdateHistoryEntity> History { get; } = history;
}