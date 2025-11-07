using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.ReflectionAndSupport.CustomFields.Data;
using NouAssist.ReflectionAndSupport.CustomFields.Entities;
using NouAssist.ReflectionAndSupport.CustomFields.Permissions;
using NouAssist.ReflectionAndSupport.CustomFields.Services;
using NouAssist.ReflectionAndSupport.CustomFields.Values.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace NouAssist.ReflectionAndSupport.CustomFields.Values
{
    [Authorize(CustomFieldsPermissions.Values.Default)]
    public class CustomFieldValueAppService : ApplicationService, ICustomFieldValueAppService
    {
        private readonly ICustomFieldValueRepository _valueRepository;
        private readonly ICustomFieldDefinitionRepository _definitionRepository;
        private readonly CustomFieldValidationService _validationService;

        public CustomFieldValueAppService(
            ICustomFieldValueRepository valueRepository,
            ICustomFieldDefinitionRepository definitionRepository,
            CustomFieldValidationService validationService)
        {
            _valueRepository = valueRepository;
            _definitionRepository = definitionRepository;
            _validationService = validationService;
        }

        public async Task<EntityCustomFieldValuesDto> GetByEntityAsync(
            string entityType, 
            Guid entityId)
        {
            var values = await _valueRepository.GetByEntityAsync(entityType, entityId);
            var definitions = await _definitionRepository.GetListAsync(
                x => values.Select(v => v.FieldDefinitionId).Contains(x.Id));

            var valueDtos = new List<CustomFieldValueDto>();
            foreach (var value in values)
            {
                var dto = ObjectMapper.Map<CustomFieldValue, CustomFieldValueDto>(value);
                var definition = definitions.FirstOrDefault(d => d.Id == value.FieldDefinitionId);
                if (definition != null)
                {
                    dto.FieldName = definition.Name;
                    dto.FieldKey = definition.FieldKey;
                }
                valueDtos.Add(dto);
            }

            return new EntityCustomFieldValuesDto
            {
                EntityType = entityType,
                EntityId = entityId,
                Values = valueDtos
            };
        }

        [Authorize(CustomFieldsPermissions.Values.Manage)]
        public async Task<CustomFieldValueDto> SetValueAsync(
            string entityType, 
            Guid entityId, 
            SetCustomFieldValueDto input)
        {
            var definition = await _definitionRepository.GetAsync(input.FieldDefinitionId);

            // Validate value
            definition.ValidateValue(input.Value);

            // Check if value exists
            var existingValue = await _valueRepository.GetByFieldAndEntityAsync(
                input.FieldDefinitionId,
                entityType,
                entityId
            );

            var valueJson = System.Text.Json.JsonSerializer.Serialize(input.Value);

            if (existingValue != null)
            {
                existingValue.UpdateValue(valueJson);
                await _valueRepository.UpdateAsync(existingValue);
                
                var dto = ObjectMapper.Map<CustomFieldValue, CustomFieldValueDto>(existingValue);
                dto.FieldName = definition.Name;
                dto.FieldKey = definition.FieldKey;
                return dto;
            }
            else
            {
                var newValue = new CustomFieldValue(
                    GuidGenerator.Create(),
                    input.FieldDefinitionId,
                    entityType,
                    entityId,
                    valueJson,
                    CurrentTenant.Id
                );

                await _valueRepository.InsertAsync(newValue);

                var dto = ObjectMapper.Map<CustomFieldValue, CustomFieldValueDto>(newValue);
                dto.FieldName = definition.Name;
                dto.FieldKey = definition.FieldKey;
                return dto;
            }
        }

        [Authorize(CustomFieldsPermissions.Values.Manage)]
        public async Task BulkSetValuesAsync(BulkSetCustomFieldValuesDto input)
        {
            // Load all definitions
            var definitionIds = input.FieldValues.Keys.ToList();
            var definitions = await _definitionRepository.GetListAsync(
                x => definitionIds.Contains(x.Id));

            // Validate all values
            var validationResult = _validationService.ValidateCustomFields(
                input.FieldValues,
                definitions
            );

            if (!validationResult.IsValid)
            {
                throw new UserFriendlyException(
                    string.Join(", ", validationResult.Errors.Select(e => e.Message)));
            }

            // Set each value
            foreach (var kvp in input.FieldValues)
            {
                await SetValueAsync(
                    input.EntityType,
                    input.EntityId,
                    new SetCustomFieldValueDto
                    {
                        FieldDefinitionId = kvp.Key,
                        Value = kvp.Value
                    }
                );
            }
        }

        [Authorize(CustomFieldsPermissions.Values.Manage)]
        public async Task DeleteValueAsync(
            string entityType, 
            Guid entityId, 
            Guid fieldDefinitionId)
        {
            var value = await _valueRepository.GetByFieldAndEntityAsync(
                fieldDefinitionId,
                entityType,
                entityId
            );

            if (value != null)
            {
                await _valueRepository.DeleteAsync(value);
            }
        }

        [Authorize(CustomFieldsPermissions.Values.Manage)]
        public async Task DeleteAllValuesAsync(string entityType, Guid entityId)
        {
            await _valueRepository.DeleteByEntityAsync(entityType, entityId);
        }
    }
}
