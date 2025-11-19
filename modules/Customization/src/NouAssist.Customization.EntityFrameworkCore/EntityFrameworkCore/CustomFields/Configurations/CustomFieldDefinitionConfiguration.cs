using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NouAssist.Customization.CustomFields.Aggregates;
using NouAssist.Customization.CustomFields.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Customization.EntityFrameworkCore.CustomFields.Configurations
{
    public class CustomFieldDefinitionConfiguration : IEntityTypeConfiguration<CustomFieldDefinition>
    {
        public void Configure(EntityTypeBuilder<CustomFieldDefinition> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                CustomizationDbProperties.DbTablePrefix + "CustomFieldDefinitions",
                CustomizationDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.OwnerId).IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.FieldKey)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.FieldType)
                .IsRequired()
                .HasConversion<byte>();

            builder.Property(x => x.EntityType)
                .IsRequired()
                .HasConversion<byte>();

            builder.Property(x => x.ValidationRegex)
                .HasMaxLength(500);

            builder.Property(x => x.ValidationMessage)
                .HasMaxLength(500);

            builder.Property(x => x.DefaultValue)
                .HasMaxLength(500);

            builder.Property(x => x.Placeholder)
                .HasMaxLength(200);

            builder.Property(x => x.HelpText)
                .HasMaxLength(500);

            builder.Property(x => x.Icon)
                .HasMaxLength(100);

            builder.Property(x => x.ShowIfValue)
                .HasMaxLength(100);

            // Options as JSON
            // 1. Define the Converter (Fixes Type Inference & Ambiguity)
            var optionsConverter = new ValueConverter<List<CustomFieldOption>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<CustomFieldOption>>(v, (JsonSerializerOptions?)null) ??
                     new List<CustomFieldOption>()
            );

            // 2. Define the Comparer (Fixes Change Tracking)
            // Without this, adding/removing items from the list won't be saved to the DB
            var optionsComparer = new ValueComparer<List<CustomFieldOption>>(
                (c1, c2) => JsonSerializer.Serialize(c1, (JsonSerializerOptions?)null) ==
                            JsonSerializer.Serialize(c2, (JsonSerializerOptions?)null),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => JsonSerializer.Deserialize<List<CustomFieldOption>>(
                    JsonSerializer.Serialize(c, (JsonSerializerOptions?)null), (JsonSerializerOptions?)null)
            );

            builder.Property("_options")
                .HasColumnName("Options")
                .HasColumnType("jsonb")
                .HasConversion(optionsConverter) // Apply Converter
                .Metadata.SetValueComparer(optionsComparer); // Apply Comparer
            // Indexes
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.FieldKey);
            builder.HasIndex(x => x.EntityType);
            builder.HasIndex(x => new { x.EntityType, x.IsActive });
            builder.HasIndex(x => x.TenantId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}