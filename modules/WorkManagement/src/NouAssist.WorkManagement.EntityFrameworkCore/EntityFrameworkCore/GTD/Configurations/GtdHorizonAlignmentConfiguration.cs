using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.GTD.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.GTD.Configurations
{
    public class GtdHorizonAlignmentConfiguration : IEntityTypeConfiguration<GtdHorizonAlignment>
    {
        public void Configure(EntityTypeBuilder<GtdHorizonAlignment> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "HorizonAlignments",
                WorkManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId).IsRequired();
            builder.Property(x => x.HigherHorizonId).IsRequired();
            
            builder.Property(x => x.HigherLevel)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.LowerEntityType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.LowerEntityId).IsRequired();
            builder.Property(x => x.AlignmentStrength);
            builder.Property(x => x.AlignmentNotes).HasMaxLength(2000);
            builder.Property(x => x.LastValidatedAt);
            builder.Property(x => x.ValidationNotes).HasMaxLength(2000);
            builder.Property(x => x.IsValidated).IsRequired();

            builder.HasIndex(x => new { x.HigherHorizonId, x.LowerEntityType, x.LowerEntityId });
            builder.HasIndex(x => x.OwnerId);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
