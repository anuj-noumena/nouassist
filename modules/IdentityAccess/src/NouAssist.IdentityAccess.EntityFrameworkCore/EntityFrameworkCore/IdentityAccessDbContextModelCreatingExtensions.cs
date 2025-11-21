using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.IdentityAccess.EntityFrameworkCore;

public static class IdentityAccessDbContextModelCreatingExtensions
{
    public static void ConfigureIdentityAccess(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

       

        builder.ApplyConfigurationsFromAssembly(typeof(IdentityAccessDbContext).Assembly);
    }
}
