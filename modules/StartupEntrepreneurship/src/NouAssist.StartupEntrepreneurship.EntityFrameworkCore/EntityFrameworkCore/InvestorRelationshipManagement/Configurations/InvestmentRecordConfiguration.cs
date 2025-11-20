using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for InvestmentRecord entity
    /// </summary>
    public class InvestmentRecordConfiguration : IEntityTypeConfiguration<InvestmentRecord>
    {
        public void Configure(EntityTypeBuilder<InvestmentRecord> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(IRMDbProperties.DbTablePrefix + "InvestmentRecords", IRMDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.InvestorId)
                .IsRequired();

            builder.Property(x => x.DocumentReference)
                .HasMaxLength(200);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            // Value Object Mapping - CommitmentDetails
            builder.OwnsOne(x => x.Commitment, commitmentBuilder =>
            {
                commitmentBuilder.Property(c => c.InvestedAmount)
                    .IsRequired()
                    .HasColumnName("InvestedAmount")
                    .HasColumnType("decimal(18,2)");

                commitmentBuilder.Property(c => c.CommittedAmount)
                    .IsRequired()
                    .HasColumnName("CommittedAmount")
                    .HasColumnType("decimal(18,2)");

                commitmentBuilder.Property(c => c.OwnershipPercentage)
                    .IsRequired()
                    .HasColumnName("OwnershipPercentage")
                    .HasColumnType("decimal(5,2)");

                commitmentBuilder.Property(c => c.InvestmentRound)
                    .IsRequired()
                    .HasColumnName("InvestmentRound")
                    .HasMaxLength(100);

                commitmentBuilder.Property(c => c.InvestmentDate)
                    .IsRequired()
                    .HasColumnName("InvestmentDate");
            });

            // Indexes
            builder.HasIndex(x => x.InvestorId)
                .HasDatabaseName("IX_InvestmentRecords_InvestorId");

            builder.HasIndex(x => x.IsActive)
                .HasDatabaseName("IX_InvestmentRecords_IsActive");

            builder.HasIndex("Commitment_InvestmentDate")
                .HasDatabaseName("IX_InvestmentRecords_InvestmentDate");
        }
    }
}
