using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;
using NouAssist.BusinessOperations.FreelanceOperations.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore.Configurations
{
    public class TimeEntryConfiguration : IEntityTypeConfiguration<TimeEntry>
    {
        public void Configure(EntityTypeBuilder<TimeEntry> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(FreelanceOperationsDbProperties.DbTablePrefix + "TimeEntries", 
                            FreelanceOperationsDbProperties.DbSchema);

            // Composite Key (using GetKeys() method from entity)
            builder.HasKey("ProjectId", nameof(TimeEntry.Date), nameof(TimeEntry.StartTime));

            // Property Mappings
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.StartTime).IsRequired();
            builder.Property(x => x.DurationMinutes).IsRequired();
            
            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(FreelanceOperationsConsts.TimeEntry.MaxDescriptionLength);

            builder.Property(x => x.IsBillable).IsRequired();
            builder.Property(x => x.IsInvoiced).IsRequired();
            builder.Property(x => x.InvoiceId);

            // Indexing
            builder.HasIndex("ProjectId");
            builder.HasIndex(x => x.Date);
            builder.HasIndex(x => x.IsInvoiced);
        }
    }
}
