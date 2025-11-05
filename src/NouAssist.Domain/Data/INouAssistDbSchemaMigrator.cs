using System.Threading.Tasks;

namespace NouAssist.Data;

public interface INouAssistDbSchemaMigrator
{
    Task MigrateAsync();
}
