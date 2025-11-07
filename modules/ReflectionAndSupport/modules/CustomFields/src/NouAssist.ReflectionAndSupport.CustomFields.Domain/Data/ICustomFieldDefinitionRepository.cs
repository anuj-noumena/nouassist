using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.ReflectionAndSupport.CustomFields.Aggregates;
using NouAssist.ReflectionAndSupport.CustomFields.Enums;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.ReflectionAndSupport.CustomFields.Data
{
    public interface ICustomFieldDefinitionRepository : IRepository<CustomFieldDefinition, Guid>
    {
        Task<List<CustomFieldDefinition>> GetByEntityTypeAsync(CustomFieldEntityType entityType);
        Task<List<CustomFieldDefinition>> GetActiveFieldsAsync(CustomFieldEntityType entityType);
        Task<CustomFieldDefinition> GetByFieldKeyAsync(string fieldKey);
        Task<List<CustomFieldDefinition>> GetByOwnerAsync(Guid ownerId);
        Task<List<CustomFieldDefinition>> GetListAsync(ISpecification<CustomFieldDefinition> spec);
    }
}
