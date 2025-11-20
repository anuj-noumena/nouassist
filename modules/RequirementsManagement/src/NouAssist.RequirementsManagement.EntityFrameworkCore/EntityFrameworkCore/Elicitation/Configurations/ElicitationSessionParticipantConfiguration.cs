using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Elicitation.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore.Configurations
{
    public class ElicitationSessionParticipantConfiguration : IEntityTypeConfiguration<ElicitationSessionParticipant>
    {
        public void Configure(EntityTypeBuilder<ElicitationSessionParticipant> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ElicitationDbProperties.DbTablePrefix + "SessionParticipants", 
                           ElicitationDbProperties.DbSchema);

            // Composite Key
            builder.HasKey(x => new { x.SessionId, x.StakeholderId });

            // Properties
            builder.Property(x => x.SessionId)
                .IsRequired();

            builder.Property(x => x.StakeholderId)
                .IsRequired();

            builder.Property(x => x.IsRequired)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.HasAttended)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.AttendedAt)
                .IsRequired(false);

            // Indexes
            builder.HasIndex(x => x.SessionId)
                .HasDatabaseName("IX_SessionParticipants_SessionId");

            builder.HasIndex(x => x.StakeholderId)
                .HasDatabaseName("IX_SessionParticipants_StakeholderId");

            builder.HasIndex(x => x.IsRequired)
                .HasDatabaseName("IX_SessionParticipants_IsRequired");

            builder.HasIndex(x => x.HasAttended)
                .HasDatabaseName("IX_SessionParticipants_HasAttended");
        }
    }
}
