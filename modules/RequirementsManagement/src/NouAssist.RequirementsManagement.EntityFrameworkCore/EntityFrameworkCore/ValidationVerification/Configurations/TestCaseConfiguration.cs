using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.ValidationVerification.Aggregates;
using NouAssist.RequirementsManagement.ValidationVerification.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.ValidationVerification.EntityFrameworkCore.Configurations
{
    public class TestCaseConfiguration : IEntityTypeConfiguration<TestCase>
    {
        public void Configure(EntityTypeBuilder<TestCase> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention(); 

            // Table Mapping
            builder.ToTable(ValidationVerificationDbProperties.DbTablePrefix + "TestCases", 
                            ValidationVerificationDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ValidationVerificationConsts.TestCase.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(ValidationVerificationConsts.TestCase.MaxDescriptionLength);

            builder.Property(x => x.Preconditions)
                .HasMaxLength(ValidationVerificationConsts.TestCase.MaxPreconditionsLength);

            builder.Property(x => x.Postconditions)
                .HasMaxLength(ValidationVerificationConsts.TestCase.MaxPostconditionsLength);

            // Enum Mappings
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Priority)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // TimeSpan mapping
            builder.Property(x => x.EstimatedDuration)
                .IsRequired(false);

            // Collection Mapping - RequirementIds as JSON
            builder.Property(x => x.RequirementIds)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v.ToList(), (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null).AsReadOnly())
                .HasColumnType("jsonb")
                .HasColumnName("RequirementIds");

            // Navigation Property - TestSteps
            builder.HasMany(x => x.TestSteps)
                   .WithOne()
                   .HasForeignKey(ts => ts.TestCaseId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Navigation Property - ExecutionLogs
            builder.HasMany(x => x.ExecutionLogs)
                   .WithOne()
                   .HasForeignKey(el => el.TestCaseId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.Priority);

            // Extra Properties
            builder.Property(x => x.ExtraProperties).HasColumnName(nameof(TestCase.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
