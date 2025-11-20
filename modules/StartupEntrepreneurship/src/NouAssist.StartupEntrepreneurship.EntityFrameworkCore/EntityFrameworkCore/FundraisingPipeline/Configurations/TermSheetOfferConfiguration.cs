using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore.Configurations
{
    public class TermSheetOfferConfiguration : IEntityTypeConfiguration<TermSheetOffer>
    {
        public void Configure(EntityTypeBuilder<TermSheetOffer> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                FundraisingPipelineDbProperties.DbTablePrefix + "TermSheetOffers", 
                FundraisingPipelineDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(FundraisingPipelineConsts.TermSheet.MaxTitleLength);

            builder.Property(x => x.ProspectId)
                .IsRequired();

            builder.Property(x => x.FundraisingRoundId)
                .IsRequired(false);

            builder.Property(x => x.ReceivedDate)
                .IsRequired();

            builder.Property(x => x.ExpirationDate)
                .IsRequired(false);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Value Object Mapping - TermComparisonData
            builder.OwnsOne(x => x.TermData, termDataBuilder =>
            {
                termDataBuilder.Property(t => t.PreMoneyValuation)
                    .HasColumnName("PreMoneyValuation")
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                termDataBuilder.Property(t => t.PostMoneyValuation)
                    .HasColumnName("PostMoneyValuation")
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                termDataBuilder.Property(t => t.InvestmentAmount)
                    .HasColumnName("InvestmentAmount")
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                termDataBuilder.Property(t => t.EquityPercentage)
                    .HasColumnName("EquityPercentage")
                    .IsRequired()
                    .HasColumnType("decimal(5,2)");

                termDataBuilder.Property(t => t.HasLiquidationPreference)
                    .HasColumnName("HasLiquidationPreference")
                    .IsRequired();

                termDataBuilder.Property(t => t.LiquidationMultiple)
                    .HasColumnName("LiquidationMultiple")
                    .IsRequired()
                    .HasColumnType("decimal(3,2)");

                termDataBuilder.Property(t => t.HasParticipationRights)
                    .HasColumnName("HasParticipationRights")
                    .IsRequired();

                termDataBuilder.Property(t => t.HasProRataRights)
                    .HasColumnName("HasProRataRights")
                    .IsRequired();

                termDataBuilder.Property(t => t.HasAntiDilutionProtection)
                    .HasColumnName("HasAntiDilutionProtection")
                    .IsRequired();

                // For the InvestorRights dictionary, we'll use JSON serialization
                termDataBuilder.Property(t => t.InvestorRights)
                    .HasColumnName("InvestorRights")
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, bool>>(v, (JsonSerializerOptions)null) 
                            ?? new Dictionary<string, bool>())
                    .HasColumnType("jsonb");
            });

            // Relationship Mappings
            builder.HasMany(x => x.Clauses)
                .WithOne()
                .HasForeignKey(c => c.TermSheetId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.NegotiationNotes)
                .WithOne()
                .HasForeignKey(n => n.TermSheetId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.ProspectId);
            builder.HasIndex(x => x.FundraisingRoundId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.ReceivedDate);
            builder.HasIndex(x => x.ExpirationDate);
        }
    }
}
