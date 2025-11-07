using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.ReflectionAndSupport.CustomFields.Definitions.Dtos;
using NouAssist.ReflectionAndSupport.CustomFields.Enums;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.ReflectionAndSupport.CustomFields.Definitions
{
    public interface ICustomFieldDefinitionAppService : IApplicationService
    {
        Task<CustomFieldDefinitionDto> GetAsync(Guid id);
        Task<PagedResultDto<CustomFieldDefinitionDto>> GetListAsync(GetCustomFieldDefinitionListDto input);
        Task<List<CustomFieldDefinitionDto>> GetByEntityTypeAsync(CustomFieldEntityType entityType);
        Task<List<CustomFieldDefinitionDto>> GetActiveFieldsAsync(CustomFieldEntityType entityType);
        Task<CustomFieldDefinitionDto> CreateAsync(CreateCustomFieldDefinitionDto input);
        Task<CustomFieldDefinitionDto> UpdateAsync(Guid id, UpdateCustomFieldDefinitionDto input);
        Task DeleteAsync(Guid id);
        Task<CustomFieldDefinitionDto> ActivateAsync(Guid id);
        Task<CustomFieldDefinitionDto> DeactivateAsync(Guid id);
        Task<CustomFieldOptionDto> AddOptionAsync(Guid id, CreateCustomFieldOptionDto input);
        Task RemoveOptionAsync(Guid id, Guid optionId);
    }
}
