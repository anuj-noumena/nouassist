namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for CustomerSegment entity
    /// </summary>
    public class CustomerSegmentConfiguration : IEntityTypeConfiguration<CustomerSegment>
    {
        public void Configure(EntityTypeBuilder<CustomerSegment> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(ProductDiscoveryDbProperties.DbTablePrefix + "CustomerSegments", 
                           ProductDiscoveryDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ProductDiscoveryConsts.MaxSegmentNameLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(ProductDiscoveryConsts.MaxSegmentDescriptionLength);

            builder.Property(x => x.TargetSampleSize)
                .IsRequired();

            builder.Property(x => x.ActualSampleSize)
                .IsRequired()
                .HasDefaultValue(0);

            // Configure SegmentCriteria value object as owned type
            builder.OwnsOne(x => x.Criteria, criteriaBuilder =>
            {
                criteriaBuilder.Property(c => c.MinimumSampleSize)
                    .HasColumnName("CriteriaMinimumSampleSize")
                    .IsRequired();

                // Store criteria dictionary as JSON
                criteriaBuilder.Property(c => c.Criteria)
                    .HasColumnName("CriteriaSettings")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                        v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, JsonSerializerOptions.Default) ?? new Dictionary<string, object>())
                    .HasColumnType("jsonb");
            });

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.TargetSampleSize);
            builder.HasIndex(x => x.ActualSampleSize);

            // Configure ExtraProperties and multi-tenancy
            builder.ConfigureExtraProperties();
            builder.ConfigureByConvention();
        }
    }
}
