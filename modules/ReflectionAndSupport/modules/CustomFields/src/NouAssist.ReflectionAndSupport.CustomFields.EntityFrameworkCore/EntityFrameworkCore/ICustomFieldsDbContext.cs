using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ReflectionAndSupport.CustomFields.EntityFrameworkCore;

[ConnectionStringName(CustomFieldsDbProperties.ConnectionStringName)]
public interface ICustomFieldsDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
