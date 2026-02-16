using System.Diagnostics.CodeAnalysis;

namespace MTPCoverageIssue.Database;

[ExcludeFromCodeCoverage]
public class UpdateHistoryEntity
{
    public int Id { get; set; }

    public string ApplicationName { get; set; } = null!;

    public string MemberName { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public Version Version { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime UpdateDateTime { get; set; }
}