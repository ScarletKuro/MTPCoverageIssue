namespace MTPCoverageIssue.Model;

public class CountGroupByVersion(Version version, long count, IEnumerable<CountGroupByVersion.Membership> memberships)
{
    public class Membership(Version version, string memberName)
    {
        public Version Version { get; } = version;

        public string MemberName { get; } = memberName;
    }

    public Version Version { get; } = version;

    public long Count { get; } = count;

    public IEnumerable<Membership> Memberships { get; } = memberships;
}