using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsReceivable.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.AccountsReceivable
{
    public class CollectionActivityConfiguration : IEntityTypeConfiguration<CollectionActivity>
    {
        public void Configure(EntityTypeBuilder<CollectionActivity> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "CollectionActivities",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => new { x.CaseId, x.ActivityDate, x.LoggedBy });

            builder.Property(x => x.CaseId).IsRequired();
            builder.Property(x => x.ActivityType).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.ActivityDate).IsRequired();
            builder.Property(x => x.ContactedPerson);
            builder.Property(x => x.FollowUpDate);
            builder.Property(x => x.LoggedBy).IsRequired();

            builder.HasIndex(x => x.CaseId).HasDatabaseName("IX_CollectionActivities_CaseId");
            builder.HasIndex(x => x.ActivityDate).HasDatabaseName("IX_CollectionActivities_ActivityDate");
        }
    }
}
