using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Users.EntityFrameworkCore
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
