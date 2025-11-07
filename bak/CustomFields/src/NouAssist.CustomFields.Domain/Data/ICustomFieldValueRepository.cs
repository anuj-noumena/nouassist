using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.CustomFields.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CustomFields.Data
{
    public interface ICustomFieldValueRepository : IRepository<CustomFieldValue, Guid>
    {
        Task<List<CustomFieldValue>> GetByEntityAsync(string entityType, Guid entityId);
        Task<CustomFieldValue> GetByFieldAndEntityAsync(
            Guid fieldDefinitionId, 
            string entityType, 
            Guid entityId
        );
        Task<Dictionary<Guid, CustomFieldValue>> GetValuesForEntityAsync(
            string entityType, 
            Guid entityId
        );
        Task DeleteByEntityAsync(string entityType, Guid entityId);
    }
}
