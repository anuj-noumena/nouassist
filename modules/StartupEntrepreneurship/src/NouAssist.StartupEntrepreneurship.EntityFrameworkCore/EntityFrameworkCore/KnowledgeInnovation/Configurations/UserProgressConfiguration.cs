using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Configurations
{
    public class UserProgressConfiguration : IEntityTypeConfiguration<UserProgress>
    {
        public void Configure(EntityTypeBuilder<UserProgress> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(KnowledgeInnovationDbProperties.DbTablePrefix + "UserProgresses",
                            KnowledgeInnovationDbProperties.DbSchema);

            builder.HasKey(x => new { x.LearningResourceId, x.UserId });

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ProgressPercentage)
                .IsRequired();

            builder.Property(x => x.LastAccessedAt)
                .IsRequired();

            builder.Property(x => x.CompletedAt);

            builder.HasIndex(x => new { x.UserId, x.Status });
            builder.HasIndex(x => x.TenantId);
        }
    }
}
