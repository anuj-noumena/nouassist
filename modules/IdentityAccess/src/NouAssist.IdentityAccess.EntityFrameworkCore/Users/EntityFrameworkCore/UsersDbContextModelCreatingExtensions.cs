using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.IdentityAccess.Users.EntityFrameworkCore
{
    public static class UsersDbContextModelCreatingExtensions
    {
        public static void ConfigureUsers(
            this ModelBuilder builder,
            UsersDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
        }
    }
}
