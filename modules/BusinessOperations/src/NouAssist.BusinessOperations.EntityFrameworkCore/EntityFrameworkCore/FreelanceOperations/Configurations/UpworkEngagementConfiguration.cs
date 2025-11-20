using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore.Configurations
{
    public class UpworkEngagementConfiguration : IEntityTypeConfiguration<UpworkEngagement>
    {
        public void Configure(EntityTypeBuilder<UpworkEngagement> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(FreelanceOperationsDbProperties.DbTablePrefix + "UpworkEngagements", 
                            FreelanceOperationsDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.UpworkJobId)
                .IsRequired()
                .HasMaxLength(FreelanceOperationsConsts.Upwork.MaxJobIdLength);
                
            builder.Property(x => x.UpworkContractId)
                .HasMaxLength(FreelanceOperationsConsts.Upwork.MaxContractIdLength);
                
            builder.Property(x => x.JobTitle)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(x => x.ClientName)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(x => x.JobLink)
                .IsRequired();
                
            builder.Property(x => x.ContractType)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(x => x.StartDate);
            builder.Property(x => x.EndDate);
            builder.Property(x => x.LinkedProjectId);
            builder.Property(x => x.LinkedClientId);

            // Indexing
            builder.HasIndex(x => x.UpworkJobId).IsUnique();
            builder.HasIndex(x => x.UpworkContractId).IsUnique();
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.LinkedProjectId);
            builder.HasIndex(x => x.LinkedClientId);
        }
    }
}
