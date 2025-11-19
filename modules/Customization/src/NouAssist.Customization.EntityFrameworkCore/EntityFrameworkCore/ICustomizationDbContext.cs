using Microsoft.EntityFrameworkCore;
using NouAssist.Customization.CustomFields.Aggregates;
using NouAssist.Customization.CustomFields.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Customization.EntityFrameworkCore;

[ConnectionStringName(CustomizationDbProperties.ConnectionStringName)]
public interface ICustomizationDbContext : IEfCoreDbContext
{
    DbSet<CustomFieldDefinition> CustomFieldDefinitions { get; }
    DbSet<CustomFieldValue> CustomFieldValues { get; }
}
