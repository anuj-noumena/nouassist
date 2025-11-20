using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Elicitation.Aggregates;
using NouAssist.RequirementsManagement.Elicitation.Constants;
using NouAssist.RequirementsManagement.Elicitation.Entities;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore.Configurations
{
    public class ElicitationSessionConfiguration : IEntityTypeConfiguration<ElicitationSession>
    {
        public void Configure(EntityTypeBuilder<ElicitationSession> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ElicitationDbProperties.DbTablePrefix + "ElicitationSessions", 
                           ElicitationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Basic Properties
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(ElicitationConsts.MaxSessionTitleLength);

            builder.Property(x => x.Description)
                .HasMaxLength(ElicitationConsts.MaxSessionDescriptionLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.OrganizerId)
                .IsRequired();

            // SessionSchedule Value Object
            builder.OwnsOne(x => x.Schedule, scheduleBuilder =>
            {
                scheduleBuilder.Property(s => s.StartTime)
                    .IsRequired()
                    .HasColumnName("StartTime");

                scheduleBuilder.Property(s => s.EndTime)
                    .IsRequired()
                    .HasColumnName("EndTime");

                scheduleBuilder.Property(s => s.Location)
                    .HasColumnName("Location")
                    .HasMaxLength(ElicitationConsts.MaxLocationLength);

                scheduleBuilder.Property(s => s.TimeZone)
                    .IsRequired()
                    .HasColumnName("TimeZone")
                    .HasMaxLength(50)
                    .HasDefaultValue("UTC");
            });

            // Relationships
            builder.HasMany(x => x.Participants)
                .WithOne()
                .HasForeignKey(p => p.SessionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.GeneratedArtifacts)
                .WithOne()
                .HasForeignKey(a => a.SessionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SurveyDefinition)
                .WithOne()
                .HasForeignKey<SurveyDefinition>(s => s.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.InterviewRecord)
                .WithOne()
                .HasForeignKey<InterviewRecord>(i => i.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Type)
                .HasDatabaseName("IX_ElicitationSessions_Type");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_ElicitationSessions_Status");

            builder.HasIndex(x => x.OrganizerId)
                .HasDatabaseName("IX_ElicitationSessions_OrganizerId");

            builder.HasIndex(x => x.Schedule.StartTime)
                .HasDatabaseName("IX_ElicitationSessions_StartTime");

            builder.HasIndex(x => x.Schedule.EndTime)
                .HasDatabaseName("IX_ElicitationSessions_EndTime");

            // Composite indexes
            builder.HasIndex(x => new { x.Type, x.Status })
                .HasDatabaseName("IX_ElicitationSessions_Type_Status");

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(ElicitationSession.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
