using MTPCoverageIssue.Database;

namespace MTPCoverageIssue.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<UpdateHistoryEntity> UpdateHistoryFrom(this IQueryable<UpdateHistoryEntity> query, DateTime? dateTimeFrom)
    {
        if (dateTimeFrom is null)
        {
            return query;
        }

        return query.Where(x => x.UpdateDateTime >= dateTimeFrom);
    }
}
