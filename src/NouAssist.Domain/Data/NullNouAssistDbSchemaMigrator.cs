using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace NouAssist.Data;

/* This is used if database provider does't define
 * INouAssistDbSchemaMigrator implementation.
 */
public class NullNouAssistDbSchemaMigrator : INouAssistDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
