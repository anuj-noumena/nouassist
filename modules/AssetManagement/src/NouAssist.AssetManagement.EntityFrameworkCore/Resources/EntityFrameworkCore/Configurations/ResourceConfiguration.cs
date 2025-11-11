using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Resources.Aggregates;
using NouAssist.Resources.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Resources.EntityFrameworkCore.Configurations
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ResourcesDbProperties.DbTablePrefix + "Resources", 
                           ResourcesDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId).IsRequired();

            builder.Property(x => x.Kind)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Title)
                .HasMaxLength(500);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.MimeType)
                .HasMaxLength(255);

            builder.Property(x => x.Checksum)
                .HasMaxLength(128);

            builder.Property(x => x.Visibility)
                .IsRequired()
                .HasMaxLength(50);

            // Owned collection: ResourceVersion
            builder.OwnsMany(x => x.Versions, version =>
            {
                version.ToTable(ResourcesDbProperties.DbTablePrefix + "ResourceVersions");
                
                version.Property(v => v.VersionNumber).IsRequired();
                
                version.Property(v => v.StorageKey)
                    .IsRequired()
                    .HasMaxLength(500);

                version.Property(v => v.MimeType)
                    .HasMaxLength(255);

                version.Property(v => v.Checksum)
                    .HasMaxLength(128);

                version.Property(v => v.OcrStatus)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                version.Property(v => v.OcrText)
                    .HasColumnType("text");

                version.Property(v => v.PreviewStorageKey)
                    .HasMaxLength(500);

                version.Property(v => v.Metadata)
                    .HasColumnType("jsonb");

                version.HasIndex(v => new { v.ResourceId, v.VersionNumber });
                version.HasIndex(v => v.IsCurrent);
            });

            // Owned collection: ResourceAcl
            builder.OwnsMany(x => x.AccessControlList, acl =>
            {
                acl.ToTable(ResourcesDbProperties.DbTablePrefix + "ResourceAcls");
                
                acl.Property(a => a.GrantedToUserId).IsRequired();

                acl.Property(a => a.Permission)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                acl.HasIndex(a => new { a.ResourceId, a.GrantedToUserId });
                acl.HasIndex(a => a.ExpiresAt);
            });

            // Indexes
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.Kind);
            builder.HasIndex(x => x.Visibility);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
