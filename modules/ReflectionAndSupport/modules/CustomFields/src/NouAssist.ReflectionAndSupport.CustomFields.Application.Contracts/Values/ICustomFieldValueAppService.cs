using System;
using System.Threading.Tasks;
using NouAssist.ReflectionAndSupport.CustomFields.Values.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.ReflectionAndSupport.CustomFields.Values
{
    public interface ICustomFieldValueAppService : IApplicationService
    {
        Task<EntityCustomFieldValuesDto> GetByEntityAsync(string entityType, Guid entityId);
        Task<CustomFieldValueDto> SetValueAsync(string entityType, Guid entityId, SetCustomFieldValueDto input);
        Task BulkSetValuesAsync(BulkSetCustomFieldValuesDto input);
        Task DeleteValueAsync(string entityType, Guid entityId, Guid fieldDefinitionId);
        Task DeleteAllValuesAsync(string entityType, Guid entityId);
    }
}
