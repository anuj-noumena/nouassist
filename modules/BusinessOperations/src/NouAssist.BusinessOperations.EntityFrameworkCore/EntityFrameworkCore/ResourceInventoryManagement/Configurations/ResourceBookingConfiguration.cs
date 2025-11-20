using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceBooking;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Constants;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for ResourceBooking aggregate
    /// </summary>
    public class ResourceBookingConfiguration : IEntityTypeConfiguration<ResourceBookingAggregate>
    {
        public void Configure(EntityTypeBuilder<ResourceBookingAggregate> builder)
        {
            // Apply ABP base conventions (auditing, concurrency, etc.)
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(RIMDbProperties.DbTablePrefix + "ResourceBookings", RIMDbProperties.DbSchema);

            // Primary key (handled by Entity<Guid> / AggregateRoot<Guid>)
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.ResourceInstanceId)
                .IsRequired();

            builder.Property(x => x.BookedByUserId)
                .IsRequired();

            builder.Property(x => x.AssignedToUserId);

            builder.Property(x => x.BookingCode)
                .IsRequired()
                .HasMaxLength(RIMConstants.ResourceBookings.MaxBookingCodeLength);

            // Value Object conversions
            builder.OwnsOne(x => x.TimeSlot, timeSlotBuilder =>
            {
                timeSlotBuilder.Property(ts => ts.StartTime)
                    .HasColumnName("StartTime")
                    .IsRequired();

                timeSlotBuilder.Property(ts => ts.EndTime)
                    .HasColumnName("EndTime")
                    .IsRequired();
            });

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.Purpose)
                .HasMaxLength(RIMConstants.ResourceBookings.MaxPurposeLength);

            builder.Property(x => x.RelatedEntity)
                .HasMaxLength(100);

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            builder.Property(x => x.CancellationReason)
                .HasMaxLength(500);

            builder.Property(x => x.IsRecurring)
                .IsRequired();

            // Foreign keys
            builder.HasOne<Aggregates.ResourceInstance.ResourceInstanceAggregate>()
                .WithMany()
                .HasForeignKey(x => x.ResourceInstanceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Recursive relationship for recurring bookings
            builder.HasOne<ResourceBookingAggregate>()
                .WithMany()
                .HasForeignKey(x => x.RecurringBookingId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.BookingCode).IsUnique();
            builder.HasIndex(x => x.ResourceInstanceId);
            builder.HasIndex(x => x.BookedByUserId);
            builder.HasIndex(x => x.AssignedToUserId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex("StartTime");  // Indexing the owned entity's property
            builder.HasIndex("EndTime");    // Indexing the owned entity's property
            builder.HasIndex(x => new { x.RelatedEntity, x.RelatedEntityId });
        }
    }
}
