using Microsoft.EntityFrameworkCore;
using NouAssist.Customization.CustomFields.Aggregates;
using NouAssist.Customization.CustomFields.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Customization.CustomFields.EntityFrameworkCore
{
    [ConnectionStringName(CustomFieldsDbProperties.ConnectionStringName)]
    public interface ICustomFieldsDbContext : IEfCoreDbContext
    {
        DbSet<CustomFieldDefinition> CustomFieldDefinitions { get; }
        DbSet<CustomFieldValue> CustomFieldValues { get; }
    }
}
