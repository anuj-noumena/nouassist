using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IdentityAccess.Users.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IdentityAccess.EntityFrameworkCore.Users.Configurations
{
    public class UserEisenhowerPreferenceConfiguration : IEntityTypeConfiguration<UserEisenhowerPreference>
    {
        public void Configure(EntityTypeBuilder<UserEisenhowerPreference> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(IdentityAccessDbProperties.DbTablePrefix + "EisenhowerPreferences",
                IdentityAccessDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.ImportanceThreshold)
                .IsRequired()
                .HasPrecision(3, 2);

            builder.Property(x => x.UrgencyThreshold)
                .IsRequired()
                .HasPrecision(3, 2);

            builder.Property(x => x.UseAutoClassification)
                .IsRequired();

            builder.HasIndex(x => x.UserId).IsUnique();

            builder.ApplyObjectExtensionMappings();
        }
    }
}
