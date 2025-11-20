namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for InterviewTranscript entity
    /// </summary>
    public class InterviewTranscriptConfiguration : IEntityTypeConfiguration<InterviewTranscript>
    {
        public void Configure(EntityTypeBuilder<InterviewTranscript> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(ProductDiscoveryDbProperties.DbTablePrefix + "InterviewTranscripts", 
                           ProductDiscoveryDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.InterviewId)
                .IsRequired();

            builder.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(ProductDiscoveryConsts.MaxTranscriptContentLength);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.IsProcessed)
                .IsRequired()
                .HasDefaultValue(false);

            // Foreign key (already configured in CustomerInterview)
            builder.HasIndex(x => x.InterviewId);
            builder.HasIndex(x => x.CreatedAt);
            builder.HasIndex(x => x.IsProcessed);
        }
    }
}
