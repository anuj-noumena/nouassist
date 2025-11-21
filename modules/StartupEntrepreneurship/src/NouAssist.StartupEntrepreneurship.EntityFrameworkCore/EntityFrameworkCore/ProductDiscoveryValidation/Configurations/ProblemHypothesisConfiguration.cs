using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for ProblemHypothesis aggregate
    /// </summary>
    public class ProblemHypothesisConfiguration : IEntityTypeConfiguration<ProblemHypothesis>
    {
        public void Configure(EntityTypeBuilder<ProblemHypothesis> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(ProductDiscoveryDbProperties.DbTablePrefix + "ProblemHypotheses", 
                           ProductDiscoveryDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Context)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.RelatedCustomerSegmentId)
                .IsRequired(false);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ValidatedDate)
                .IsRequired(false);

            builder.Property(x => x.ValidationSummary)
                .HasMaxLength(ProductDiscoveryConsts.MaxValidationNotesLength);

            // Configure HypothesisStatement value object as owned type
            builder.OwnsOne(x => x.Statement, statementBuilder =>
            {
                statementBuilder.Property(s => s.Problem)
                    .HasColumnName("StatementProblem")
                    .HasMaxLength(ProductDiscoveryConsts.MaxHypothesisStatementLength)
                    .IsRequired();

                statementBuilder.Property(s => s.Target)
                    .HasColumnName("StatementTarget")
                    .HasMaxLength(ProductDiscoveryConsts.MaxHypothesisStatementLength)
                    .IsRequired();

                statementBuilder.Property(s => s.Solution)
                    .HasColumnName("StatementSolution")
                    .HasMaxLength(ProductDiscoveryConsts.MaxHypothesisStatementLength)
                    .IsRequired();

                statementBuilder.Property(s => s.ExpectedOutcome)
                    .HasColumnName("StatementExpectedOutcome")
                    .HasMaxLength(ProductDiscoveryConsts.MaxHypothesisStatementLength)
                    .IsRequired();
            });

            // Configure ProblemSolutionFitMetric value object as owned type
            builder.OwnsOne(x => x.FitMetric, fitBuilder =>
            {
                fitBuilder.Property(f => f.ProblemClarity)
                    .HasColumnName("FitProblemClarity")
                    .HasPrecision(5, 2);

                fitBuilder.Property(f => f.SolutionEffectiveness)
                    .HasColumnName("FitSolutionEffectiveness")
                    .HasPrecision(5, 2);

                fitBuilder.Property(f => f.CustomerValidation)
                    .HasColumnName("FitCustomerValidation")
                    .HasPrecision(5, 2);

                fitBuilder.Property(f => f.MarketDemand)
                    .HasColumnName("FitMarketDemand")
                    .HasPrecision(5, 2);

                fitBuilder.Property(f => f.Notes)
                    .HasColumnName("FitNotes")
                    .HasMaxLength(1000);
            });

            // Configure child entity relationships
            builder.HasMany(x => x.ValidationAttempts)
                .WithOne()
                .HasForeignKey("HypothesisId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign key to CustomerSegment
            builder.HasOne<CustomerSegment>()
                .WithMany()
                .HasForeignKey(x => x.RelatedCustomerSegmentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.RelatedCustomerSegmentId);
            builder.HasIndex(x => x.ValidatedDate);

            // Configure ExtraProperties and multi-tenancy
            builder.ConfigureExtraProperties();
            builder.ConfigureByConvention();
        }
    }
}
