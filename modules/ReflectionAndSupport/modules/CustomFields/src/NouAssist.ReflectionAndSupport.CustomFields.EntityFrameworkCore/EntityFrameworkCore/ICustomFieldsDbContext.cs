using Microsoft.EntityFrameworkCore;
using NouAssist.ReflectionAndSupport.CustomFields.Aggregates;
using NouAssist.ReflectionAndSupport.CustomFields.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ReflectionAndSupport.CustomFields.EntityFrameworkCore
{
    [ConnectionStringName(CustomFieldsDbProperties.ConnectionStringName)]
    public interface ICustomFieldsDbContext : IEfCoreDbContext
    {
        DbSet<CustomFieldDefinition> CustomFieldDefinitions { get; }
        DbSet<CustomFieldValue> CustomFieldValues { get; }
    }
}
