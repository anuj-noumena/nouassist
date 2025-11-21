using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.ValidationVerification.Entities;

namespace NouAssist.RequirementsManagement.ValidationVerification.EntityFrameworkCore.Configurations
{
    public class TestExecutionLogConfiguration : IEntityTypeConfiguration<TestExecutionLog>
    {
        public void Configure(EntityTypeBuilder<TestExecutionLog> builder)
        {
            // Table Mapping
            builder.ToTable(ValidationVerificationDbProperties.DbTablePrefix + "TestExecutionLogs", 
                            ValidationVerificationDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.TestCaseId)
                .IsRequired();

            builder.Property(x => x.ExecutorId)
                .IsRequired();

            builder.Property(x => x.ExecutorName)
                .HasMaxLength(256);

            builder.Property(x => x.StartedAt)
                .IsRequired();

            builder.Property(x => x.CompletedAt)
                .IsRequired(false);

            // Enum mapping
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ExecutionNotes)
                .HasMaxLength(2000);

            builder.Property(x => x.TotalSteps)
                .IsRequired();

            builder.Property(x => x.Environment)
                .HasMaxLength(256);

            builder.Property(x => x.TestDataUsed);

            // Computed Column - ExecutionDuration (PostgreSQL)
            builder.Property<TimeSpan?>("ExecutionDuration")
                .HasComputedColumnSql("\"CompletedAt\" - \"StartedAt\"", stored: false);

            // Indexing
            builder.HasIndex(x => x.TestCaseId);
            builder.HasIndex(x => x.ExecutorId);
            builder.HasIndex(x => x.StartedAt);
            builder.HasIndex(x => x.Status);
        }
    }
}
