using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NouAssist.Data;
using Volo.Abp.DependencyInjection;

namespace NouAssist.EntityFrameworkCore;

public class EntityFrameworkCoreNouAssistDbSchemaMigrator
    : INouAssistDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreNouAssistDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the NouAssistDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<NouAssistDbContext>()
            .Database
            .MigrateAsync();
    }
}
