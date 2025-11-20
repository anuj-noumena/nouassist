using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Configurations
{
    public class DecisionAlternativeConfiguration : IEntityTypeConfiguration<DecisionAlternative>
    {
        public void Configure(EntityTypeBuilder<DecisionAlternative> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(KnowledgeInnovationDbProperties.DbTablePrefix + "DecisionAlternatives",
                            KnowledgeInnovationDbProperties.DbSchema);

            builder.HasKey(x => new { x.DecisionRecordId, x.Title });

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.DefaultLengths.Title);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.DecisionRecord.ContextMaxLength);

            builder.Property(x => x.ProsCons)
                .HasMaxLength(2000);

            builder.Property(x => x.IsChosen)
                .IsRequired();

            builder.HasIndex(x => x.DecisionRecordId);
            builder.HasIndex(x => x.IsChosen);
        }
    }
}
