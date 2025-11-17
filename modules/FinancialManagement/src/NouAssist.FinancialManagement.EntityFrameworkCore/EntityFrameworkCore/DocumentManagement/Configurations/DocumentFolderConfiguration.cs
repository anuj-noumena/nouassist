using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.DocumentManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.DocumentManagement.EntityFrameworkCore.Configurations
{
    public class DocumentFolderConfiguration : IEntityTypeConfiguration<DocumentFolder>
    {
        public void Configure(EntityTypeBuilder<DocumentFolder> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                DocumentManagementDbProperties.DbTablePrefix + "Folders",
                DocumentManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnName(nameof(DocumentFolder.Name));

            builder.Property(x => x.Description)
                .HasMaxLength(2048)
                .HasColumnName(nameof(DocumentFolder.Description));

            builder.Property(x => x.ParentFolderId)
                .HasColumnName(nameof(DocumentFolder.ParentFolderId));

            builder.Property(x => x.Path)
                .IsRequired()
                .HasMaxLength(2048)
                .HasColumnName(nameof(DocumentFolder.Path));

            builder.Property(x => x.Level)
                .IsRequired()
                .HasColumnName(nameof(DocumentFolder.Level));

            // DocumentIds stored as JSON array
            builder.Property("_documentIds")
                .HasColumnName("DocumentIds")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.List<System.Guid>>(v, (System.Text.Json.JsonSerializerOptions)null));

            // Indexes
            builder.HasIndex(x => x.ParentFolderId)
                .HasDatabaseName("IX_DocumentFolders_ParentId");

            builder.HasIndex(x => x.Path)
                .IsUnique()
                .HasDatabaseName("IX_DocumentFolders_Path");

            builder.HasIndex(x => x.Level)
                .HasDatabaseName("IX_DocumentFolders_Level");
        }
    }
}
