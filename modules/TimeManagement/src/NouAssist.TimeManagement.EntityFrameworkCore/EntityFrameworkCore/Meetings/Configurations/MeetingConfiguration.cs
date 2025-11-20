using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NouAssist.TimeManagement.Meetings.Aggregates;
using NouAssist.TimeManagement.Meetings.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.TimeManagement.EntityFrameworkCore.Meetings.Configurations
{
    public class MeetingConfiguration : IEntityTypeConfiguration<Meeting>
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public void Configure(EntityTypeBuilder<Meeting> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                TimeManagementDbProperties.DbTablePrefix + "Meetings",
                TimeManagementDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.OwnerId).IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.Objective)
                .HasMaxLength(1000);

            builder.Property(x => x.Location)
                .HasMaxLength(500);

            builder.Property(x => x.MeetingUrl)
                .HasMaxLength(1000);

            builder.Property(x => x.CalendarEventId)
                .HasMaxLength(200);

            builder.Property(x => x.MeetingType)
                .IsRequired()
                .HasConversion<byte>();

            builder.Property(x => x.Frequency)
                .IsRequired()
                .HasConversion<byte>();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<byte>();

            builder.Property(x => x.Notes)
                .HasMaxLength(5000);

            builder.Property(x => x.Decisions)
                .HasMaxLength(5000);

            builder.Property(x => x.Summary)
                .HasMaxLength(5000);

            builder.Property(x => x.AgendaNotes)
                .HasMaxLength(5000);

            builder.Property(x => x.RecordingUrl)
                .HasMaxLength(1000);

            builder.Property(x => x.TranscriptUrl)
                .HasMaxLength(1000);

            builder.Property(x => x.CancellationReason)
                .HasMaxLength(1000);

            // Collections as JSON
            builder.Property("_attendees")
                .HasColumnName("Attendees")
                .HasColumnType("jsonb")
                .HasConversion(new ValueConverter<List<MeetingAttendee>, string>(
                    v => JsonSerializer.Serialize(v, _jsonOptions),
                    v => JsonSerializer.Deserialize<List<MeetingAttendee>>(v, _jsonOptions)
                ));

            builder.Property("_agendaItems")
                .HasColumnName("AgendaItems")
                .HasColumnType("jsonb")
                .HasConversion(new ValueConverter<List<MeetingAgendaItem>, string>(
                    v => JsonSerializer.Serialize(v, _jsonOptions),
                    v => JsonSerializer.Deserialize<List<MeetingAgendaItem>>(v, _jsonOptions)
                ));

            builder.Property("_actionItems")
                .HasColumnName("ActionItems")
                .HasColumnType("jsonb")
                .HasConversion(new ValueConverter<List<MeetingActionItem>, string>(
                    v => JsonSerializer.Serialize(v, _jsonOptions),
                    v => JsonSerializer.Deserialize<List<MeetingActionItem>>(v, _jsonOptions)
                ));

            builder.Property("_attachmentIds")
                .HasColumnName("AttachmentIds")
                .HasColumnType("jsonb")
                .HasConversion(new ValueConverter<List<Guid>, string>(
                    v => JsonSerializer.Serialize(v, _jsonOptions),
                    v => JsonSerializer.Deserialize<List<Guid>>(v, _jsonOptions)
                ));

            builder.Property("_relatedTaskIds")
                .HasColumnName("RelatedTaskIds")
                .HasColumnType("jsonb")
                .HasConversion(new ValueConverter<List<Guid>, string>(
                    v => JsonSerializer.Serialize(v, _jsonOptions),
                    v => JsonSerializer.Deserialize<List<Guid>>(v, _jsonOptions)
                ));

            builder.Property(x => x.Metadata)
                .HasColumnType("jsonb")
                .HasConversion(new ValueConverter<Dictionary<string, object>, string>(
                    v => JsonSerializer.Serialize(v, _jsonOptions),
                    v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, _jsonOptions)
                ));

            // Indexes
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.OrganizerId);
            builder.HasIndex(x => x.StartAt);
            builder.HasIndex(x => x.EndAt);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.MeetingType);
            builder.HasIndex(x => x.RecurringSeriesId);
            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => x.SprintId);
            builder.HasIndex(x => x.TenantId);
            builder.HasIndex(x => new { x.StartAt, x.EndAt });
            builder.HasIndex(x => new { x.Status, x.StartAt });

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}