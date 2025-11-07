using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.CollaborationHub.UserProfiles.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.CollaborationHub.UserProfiles.EntityFrameworkCore.Configurations
{
    public class UserAggregateConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(UsersDbProperties.DbTablePrefix + "Users",
                            UsersDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                .HasMaxLength(100);

            builder.Property(x => x.LastName)
                .HasMaxLength(100);

            builder.Property(x => x.Bio)
                .HasMaxLength(500);

            builder.Property(x => x.Avatar)
                .HasMaxLength(500);

            builder.Property(x => x.Email)
                .HasMaxLength(256);

            builder.Property(x => x.Timezone)
                .HasMaxLength(50);

            builder.Property(x => x.Locale)
                .HasMaxLength(10);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.LastLoginAt);

            builder.HasIndex(x => x.Email);
            builder.HasIndex(x => x.IsActive);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
