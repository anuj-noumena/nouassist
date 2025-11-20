using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for InvestorInteraction entity
    /// </summary>
    public class InvestorInteractionConfiguration : IEntityTypeConfiguration<InvestorInteraction>
    {
        public void Configure(EntityTypeBuilder<InvestorInteraction> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(IRMDbProperties.DbTablePrefix + "InvestorInteractions", IRMDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.InvestorId)
                .IsRequired();

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Subject)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Notes)
                .HasMaxLength(2000);

            builder.Property(x => x.Outcome)
                .HasMaxLength(1000);

            builder.Property(x => x.RelatedEntityType)
                .HasMaxLength(100);

            // Indexes
            builder.HasIndex(x => x.InvestorId)
                .HasDatabaseName("IX_InvestorInteractions_InvestorId");

            builder.HasIndex(x => x.InteractionDate)
                .HasDatabaseName("IX_InvestorInteractions_Date");

            builder.HasIndex(x => x.Type)
                .HasDatabaseName("IX_InvestorInteractions_Type");
        }
    }
}
