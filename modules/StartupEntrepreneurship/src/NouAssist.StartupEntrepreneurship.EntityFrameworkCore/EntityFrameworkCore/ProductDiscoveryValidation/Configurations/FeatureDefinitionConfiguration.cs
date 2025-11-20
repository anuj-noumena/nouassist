namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for FeatureDefinition aggregate
    /// </summary>
    public class FeatureDefinitionConfiguration : IEntityTypeConfiguration<FeatureDefinition>
    {
        public void Configure(EntityTypeBuilder<FeatureDefinition> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(ProductDiscoveryDbProperties.DbTablePrefix + "FeatureDefinitions", 
                           ProductDiscoveryDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ProductDiscoveryConsts.MaxFeatureNameLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(ProductDiscoveryConsts.MaxFeatureDescriptionLength);

            builder.Property(x => x.UserStory)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.AcceptanceCriteria)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.Priority)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.MvpStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.RelatedHypothesisId)
                .IsRequired(false);

            builder.Property(x => x.RelatedPrototypeId)
                .IsRequired(false);

            builder.Property(x => x.BusinessValue)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.TechnicalNotes)
                .HasMaxLength(2000);

            // Configure child entity relationships
            builder.HasMany(x => x.PriorityScores)
                .WithOne()
                .HasForeignKey("FeatureId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign keys to ProblemHypothesis and SolutionPrototype
            builder.HasOne<ProblemHypothesis>()
                .WithMany()
                .HasForeignKey(x => x.RelatedHypothesisId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<SolutionPrototype>()
                .WithMany()
                .HasForeignKey(x => x.RelatedPrototypeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Priority);
            builder.HasIndex(x => x.MvpStatus);
            builder.HasIndex(x => x.RelatedHypothesisId);
            builder.HasIndex(x => x.RelatedPrototypeId);

            // Configure ExtraProperties and multi-tenancy
            builder.ConfigureExtraProperties();
            builder.ConfigureByConvention();
        }
    }
}
