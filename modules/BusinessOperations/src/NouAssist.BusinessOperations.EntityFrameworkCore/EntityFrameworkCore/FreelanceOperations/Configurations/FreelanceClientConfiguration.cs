using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore.Configurations
{
    public class FreelanceClientConfiguration : IEntityTypeConfiguration<FreelanceClient>
    {
        public void Configure(EntityTypeBuilder<FreelanceClient> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(FreelanceOperationsDbProperties.DbTablePrefix + "Clients", 
                            FreelanceOperationsDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(FreelanceOperationsConsts.Client.MaxNameLength);

            builder.Property(x => x.CompanyName)
                .HasMaxLength(FreelanceOperationsConsts.Client.MaxCompanyNameLength);

            // Enum Mapping
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Value Object Mapping
            builder.OwnsOne(x => x.ContactInfo, contactInfoBuilder => 
            {
                contactInfoBuilder.Property(c => c.Email).HasColumnName("Email")
                    .IsRequired()
                    .HasMaxLength(FreelanceOperationsConsts.Client.MaxEmailLength);
                contactInfoBuilder.Property(c => c.Phone).HasColumnName("Phone")
                    .HasMaxLength(FreelanceOperationsConsts.Client.MaxPhoneLength);
                contactInfoBuilder.Property(c => c.Website).HasColumnName("Website");
                contactInfoBuilder.Property(c => c.Address).HasColumnName("Address");
            });

            // Regular Properties
            builder.Property(x => x.Notes)
                .HasMaxLength(FreelanceOperationsConsts.Client.MaxNotesLength);
            builder.Property(x => x.Source).HasMaxLength(100);
            builder.Property(x => x.RelationshipStartDate);

            // Relationship Mapping for communication records
            builder.HasMany(x => x.CommunicationRecords)
                   .WithOne()
                   .HasForeignKey(c => c.ClientId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Name);
            builder.HasIndex("ContactInfo_Email").IsUnique();
            builder.HasIndex(x => x.Status);
        }
    }
}
