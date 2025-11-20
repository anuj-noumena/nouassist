using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.FreelanceOperations.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore.Configurations
{
    public class ClientCommunicationRecordConfiguration : IEntityTypeConfiguration<ClientCommunicationRecord>
    {
        public void Configure(EntityTypeBuilder<ClientCommunicationRecord> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(FreelanceOperationsDbProperties.DbTablePrefix + "ClientCommunications", 
                            FreelanceOperationsDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.ClientId).IsRequired();
            builder.Property(x => x.ProjectId);
            builder.Property(x => x.RequiresFollowUp).IsRequired();
            builder.Property(x => x.FollowUpDate);

            // Value Object Mapping
            builder.OwnsOne(x => x.CommunicationDetails, detailsBuilder => 
            {
                detailsBuilder.Property(d => d.Timestamp).HasColumnName("Timestamp").IsRequired();
                detailsBuilder.Property(d => d.Type).HasColumnName("Type").IsRequired().HasMaxLength(50);
                detailsBuilder.Property(d => d.Subject).HasColumnName("Subject").IsRequired().HasMaxLength(200);
                detailsBuilder.Property(d => d.Content).HasColumnName("Content").IsRequired();
                detailsBuilder.Property(d => d.UserId).HasColumnName("UserId").IsRequired();
            });

            // Indexing
            builder.HasIndex(x => x.ClientId);
            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => x.RequiresFollowUp);
            builder.HasIndex(x => x.FollowUpDate);
        }
    }
}
