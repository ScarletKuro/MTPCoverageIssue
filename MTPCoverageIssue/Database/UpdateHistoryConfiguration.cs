using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MTPCoverageIssue.Database;

public class UpdateHistoryConfiguration : IEntityTypeConfiguration<UpdateHistoryEntity>
{
    public void Configure(EntityTypeBuilder<UpdateHistoryEntity> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasIndex(e => e.Id);

        builder
            .Property(e => e.ApplicationName)
            .HasMaxLength(128)
            .IsRequired();

        builder
            .HasIndex(e => e.ApplicationName);

        builder
            .Property(e => e.MemberName)
            .HasMaxLength(128)
            .IsRequired();

        builder
            .HasIndex(e => e.MemberName);

        builder
            .Property(e => e.FileName)
            .IsRequired();

        builder
            .Property(e => e.Version)
            .HasConversion(e => e.ToString(), e => new Version(e))
            .HasMaxLength(128)
            .IsRequired();

        builder
            .HasIndex(e => e.Version);
        builder
            .HasIndex(e => new { e.Version, e.MemberName });

        builder
            .Property(e => e.Status)
            .IsRequired();

        builder
            .Property(e => e.UpdateDateTime)
            .ValueGeneratedOnAdd()
            .IsRequired();
    }
}
