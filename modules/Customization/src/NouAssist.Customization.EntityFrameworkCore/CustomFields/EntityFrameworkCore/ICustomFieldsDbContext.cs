using Microsoft.EntityFrameworkCore;
using NouAssist.CustomFields.Aggregates;
using NouAssist.CustomFields.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CustomFields.EntityFrameworkCore
{
    [ConnectionStringName(CustomFieldsDbProperties.ConnectionStringName)]
    public interface ICustomFieldsDbContext : IEfCoreDbContext
    {
        DbSet<CustomFieldDefinition> CustomFieldDefinitions { get; }
        DbSet<CustomFieldValue> CustomFieldValues { get; }
    }
}
