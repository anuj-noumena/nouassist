using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.ReflectionAndSupport.CustomFields.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.ReflectionAndSupport.CustomFields.EntityFrameworkCore.Configurations
{
    public class CustomFieldValueConfiguration : IEntityTypeConfiguration<CustomFieldValue>
    {
        public void Configure(EntityTypeBuilder<CustomFieldValue> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                CustomFieldsDbProperties.DbTablePrefix + "CustomFieldValues",
                CustomFieldsDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.FieldDefinitionId).IsRequired();

            builder.Property(x => x.EntityType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.EntityId).IsRequired();

            builder.Property(x => x.ValueJson)
                .HasColumnName("Value")
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.FieldDefinitionId);
            builder.HasIndex(x => new { x.EntityType, x.EntityId });
            builder.HasIndex(x => new { x.FieldDefinitionId, x.EntityType, x.EntityId })
                .IsUnique();
            builder.HasIndex(x => x.TenantId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
