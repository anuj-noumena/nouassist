using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Elicitation.Entities;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Newtonsoft.Json;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore.Configurations
{
    public class BrainstormingIdeaConfiguration : IEntityTypeConfiguration<BrainstormingIdea>
    {
        public void Configure(EntityTypeBuilder<BrainstormingIdea> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ElicitationDbProperties.DbTablePrefix + "BrainstormingIdeas", 
                           ElicitationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.SessionId)
                .IsRequired();

            builder.Property(x => x.ContributorId)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.Priority)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(x => x.IsSelected)
                .IsRequired()
                .HasDefaultValue(false);

            // Tags as JSON array
            builder.Property(x => x.Tags)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v.ToList(), Formatting.None),
                    v => JsonConvert.DeserializeObject<List<IdeaTag>>(v))
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.SessionId)
                .HasDatabaseName("IX_BrainstormingIdeas_SessionId");

            builder.HasIndex(x => x.ContributorId)
                .HasDatabaseName("IX_BrainstormingIdeas_ContributorId");

            builder.HasIndex(x => x.Priority)
                .HasDatabaseName("IX_BrainstormingIdeas_Priority");

            builder.HasIndex(x => x.IsSelected)
                .HasDatabaseName("IX_BrainstormingIdeas_IsSelected");

            // Composite indexes
            builder.HasIndex(x => new { x.SessionId, x.IsSelected })
                .HasDatabaseName("IX_BrainstormingIdeas_Session_Selected");

            builder.HasIndex(x => new { x.SessionId, x.Priority })
                .HasDatabaseName("IX_BrainstormingIdeas_Session_Priority");
        }
    }
}
