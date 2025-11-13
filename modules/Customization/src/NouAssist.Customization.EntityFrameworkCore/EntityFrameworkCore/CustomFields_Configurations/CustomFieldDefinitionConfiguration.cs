using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Customization.CustomFields.Aggregates;
using NouAssist.Customization.CustomFields.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Customization.CustomFields.EntityFrameworkCore.Configurations
{
    public class CustomFieldDefinitionConfiguration : IEntityTypeConfiguration<CustomFieldDefinition>
    {
        public void Configure(EntityTypeBuilder<CustomFieldDefinition> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                CustomFieldsDbProperties.DbTablePrefix + "CustomFieldDefinitions",
                CustomFieldsDbProperties.DbSchema
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
            builder.Property("_options")
                .HasColumnName("Options")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<CustomFieldOption>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

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
