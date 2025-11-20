using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Elicitation.Constants;
using NouAssist.RequirementsManagement.Elicitation.Entities;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore.Configurations
{
    public class InterviewRecordConfiguration : IEntityTypeConfiguration<InterviewRecord>
    {
        public void Configure(EntityTypeBuilder<InterviewRecord> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ElicitationDbProperties.DbTablePrefix + "InterviewRecords", 
                           ElicitationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.SessionId)
                .IsRequired();

            builder.Property(x => x.RecordedAt)
                .IsRequired();

            // TranscriptRecordingRef Value Object
            builder.OwnsOne(x => x.TranscriptRecording, transcriptBuilder =>
            {
                transcriptBuilder.Property(t => t.TranscriptText)
                    .HasColumnName("TranscriptText")
                    .HasMaxLength(ElicitationConsts.MaxTranscriptLength);

                transcriptBuilder.Property(t => t.RecordingPath)
                    .HasColumnName("RecordingPath")
                    .HasMaxLength(ElicitationConsts.MaxRecordingPathLength);

                transcriptBuilder.Property(t => t.FileName)
                    .HasColumnName("FileName")
                    .HasMaxLength(ElicitationConsts.MaxFileNameLength);

                transcriptBuilder.Property(t => t.FileSizeBytes)
                    .HasColumnName("FileSizeBytes")
                    .HasDefaultValue(0);

                transcriptBuilder.Property(t => t.MimeType)
                    .HasColumnName("MimeType")
                    .HasMaxLength(100);
            });

            // Indexes
            builder.HasIndex(x => x.SessionId)
                .IsUnique()
                .HasDatabaseName("IX_InterviewRecords_SessionId");

            builder.HasIndex(x => x.RecordedAt)
                .HasDatabaseName("IX_InterviewRecords_RecordedAt");
        }
    }
}
