using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.PersonalDevelopment.EntityFrameworkCore;

public static class PersonalDevelopmentDbContextModelCreatingExtensions
{
    public static void ConfigurePersonalDevelopment(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ApplyConfigurationsFromAssembly(typeof(PersonalDevelopmentDbContext).Assembly);

    }
}
