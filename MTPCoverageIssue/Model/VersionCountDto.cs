namespace MTPCoverageIssue.Model;

public class VersionCountDto
{
    public class MembershipFlatDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("version")]
        public Version Version { get; set; } = null!;

        [System.Text.Json.Serialization.JsonPropertyName("memberName")]
        public string MemberName { get; set; } = string.Empty;
    }

    [System.Text.Json.Serialization.JsonPropertyName("version")]
    public Version Version { get; set; } = null!;

    [System.Text.Json.Serialization.JsonPropertyName("count")]
    public long Count { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("memberships")]
    public IEnumerable<MembershipFlatDto> Memberships { get; set; } = Array.Empty<MembershipFlatDto>();
}