using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.ReflectionAndSupport.CustomFields.Aggregates;
using NouAssist.ReflectionAndSupport.CustomFields.Data;
using NouAssist.ReflectionAndSupport.CustomFields.Definitions.Dtos;
using NouAssist.ReflectionAndSupport.CustomFields.Enums;
using NouAssist.ReflectionAndSupport.CustomFields.Permissions;
using NouAssist.ReflectionAndSupport.CustomFields.Specifications;
using NouAssist.ReflectionAndSupport.CustomFields.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Specifications;

namespace NouAssist.ReflectionAndSupport.CustomFields.Definitions
{
    [Authorize(CustomFieldsPermissions.Definitions.Default)]
    public class CustomFieldDefinitionAppService 
        : ApplicationService, ICustomFieldDefinitionAppService
    {
        private readonly ICustomFieldDefinitionRepository _definitionRepository;

        public CustomFieldDefinitionAppService(
            ICustomFieldDefinitionRepository definitionRepository)
        {
            _definitionRepository = definitionRepository;
        }

        public async Task<CustomFieldDefinitionDto> GetAsync(Guid id)
        {
            var definition = await _definitionRepository.GetAsync(id);
            return ObjectMapper.Map<CustomFieldDefinition, CustomFieldDefinitionDto>(definition);
        }

        public async Task<PagedResultDto<CustomFieldDefinitionDto>> GetListAsync(
            GetCustomFieldDefinitionListDto input)
        {
            var spec = CreateListSpecification(input);

            var definitions = await _definitionRepository.GetListAsync(spec);
            var totalCount = await _definitionRepository.CountAsync(spec);

            return new PagedResultDto<CustomFieldDefinitionDto>(
                totalCount,
                ObjectMapper.Map<List<CustomFieldDefinition>, List<CustomFieldDefinitionDto>>(definitions)
            );
        }

        public async Task<List<CustomFieldDefinitionDto>> GetByEntityTypeAsync(
            CustomFieldEntityType entityType)
        {
            var definitions = await _definitionRepository.GetByEntityTypeAsync(entityType);
            return ObjectMapper.Map<List<CustomFieldDefinition>, List<CustomFieldDefinitionDto>>(definitions);
        }

        public async Task<List<CustomFieldDefinitionDto>> GetActiveFieldsAsync(
            CustomFieldEntityType entityType)
        {
            var definitions = await _definitionRepository.GetActiveFieldsAsync(entityType);
            return ObjectMapper.Map<List<CustomFieldDefinition>, List<CustomFieldDefinitionDto>>(definitions);
        }

        [Authorize(CustomFieldsPermissions.Definitions.Create)]
        public async Task<CustomFieldDefinitionDto> CreateAsync(CreateCustomFieldDefinitionDto input)
        {
            var definition = new CustomFieldDefinition(
                GuidGenerator.Create(),
                CurrentUser.Id.Value,
                input.Name,
                input.FieldType,
                input.EntityType,
                CurrentTenant.Id
            );

            if (!string.IsNullOrEmpty(input.Description))
            {
                definition.SetDescription(input.Description);
            }

            if (input.IsRequired)
            {
                definition.MakeRequired();
            }

            // Add options for select fields
            if (input.Options != null && input.Options.Any())
            {
                foreach (var optionDto in input.Options)
                {
                    var option = new CustomFieldOption(
                        optionDto.Label,
                        optionDto.Value,
                        optionDto.Color
                    );
                    definition.AddOption(option);
                }
            }

            await _definitionRepository.InsertAsync(definition);

            return ObjectMapper.Map<CustomFieldDefinition, CustomFieldDefinitionDto>(definition);
        }

        [Authorize(CustomFieldsPermissions.Definitions.Update)]
        public async Task<CustomFieldDefinitionDto> UpdateAsync(
            Guid id, 
            UpdateCustomFieldDefinitionDto input)
        {
            var definition = await _definitionRepository.GetAsync(id);

            definition.SetName(input.Name);
            definition.SetDescription(input.Description);

            if (input.IsRequired)
            {
                definition.MakeRequired();
            }
            else
            {
                definition.MakeOptional();
            }

            if (!string.IsNullOrEmpty(input.ValidationRegex))
            {
                definition.SetValidationRegex(input.ValidationRegex, input.ValidationMessage);
            }

            if (!string.IsNullOrEmpty(input.DefaultValue))
            {
                definition.SetDefaultValue(input.DefaultValue);
            }

            if (definition.FieldType.IsNumeric() && 
                (input.MinValue.HasValue || input.MaxValue.HasValue))
            {
                definition.SetNumericConstraints(input.MinValue, input.MaxValue);
            }

            if (definition.FieldType.IsText() && 
                (input.MinLength.HasValue || input.MaxLength.HasValue))
            {
                definition.SetTextConstraints(input.MinLength, input.MaxLength);
            }

            if (definition.FieldType.IsDate() && 
                (input.MinDate.HasValue || input.MaxDate.HasValue))
            {
                definition.SetDateConstraints(input.MinDate, input.MaxDate);
            }

            definition.SetDisplayOrder(input.DisplayOrder);
            definition.SetDisplayProperties(input.Placeholder, input.HelpText, input.Icon);

            await _definitionRepository.UpdateAsync(definition);

            return ObjectMapper.Map<CustomFieldDefinition, CustomFieldDefinitionDto>(definition);
        }

        [Authorize(CustomFieldsPermissions.Definitions.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _definitionRepository.DeleteAsync(id);
        }

        [Authorize(CustomFieldsPermissions.Definitions.Activate)]
        public async Task<CustomFieldDefinitionDto> ActivateAsync(Guid id)
        {
            var definition = await _definitionRepository.GetAsync(id);
            definition.Activate();
            await _definitionRepository.UpdateAsync(definition);

            return ObjectMapper.Map<CustomFieldDefinition, CustomFieldDefinitionDto>(definition);
        }

        [Authorize(CustomFieldsPermissions.Definitions.Deactivate)]
        public async Task<CustomFieldDefinitionDto> DeactivateAsync(Guid id)
        {
            var definition = await _definitionRepository.GetAsync(id);
            definition.Deactivate();
            await _definitionRepository.UpdateAsync(definition);

            return ObjectMapper.Map<CustomFieldDefinition, CustomFieldDefinitionDto>(definition);
        }

        [Authorize(CustomFieldsPermissions.Definitions.Update)]
        public async Task<CustomFieldOptionDto> AddOptionAsync(
            Guid id, 
            CreateCustomFieldOptionDto input)
        {
            var definition = await _definitionRepository.GetAsync(id);

            var option = new CustomFieldOption(
                input.Label,
                input.Value,
                input.Color
            );

            definition.AddOption(option);

            await _definitionRepository.UpdateAsync(definition);

            return ObjectMapper.Map<CustomFieldOption, CustomFieldOptionDto>(option);
        }

        [Authorize(CustomFieldsPermissions.Definitions.Update)]
        public async Task RemoveOptionAsync(Guid id, Guid optionId)
        {
            var definition = await _definitionRepository.GetAsync(id);
            definition.RemoveOption(optionId);
            await _definitionRepository.UpdateAsync(definition);
        }

        private ISpecification<CustomFieldDefinition> CreateListSpecification(
            GetCustomFieldDefinitionListDto input)
        {
            var spec = Specification.None<CustomFieldDefinition>();

            if (input.EntityType.HasValue)
            {
                spec = spec.And(new CustomFieldsByEntityTypeSpecification(input.EntityType.Value));
            }

            if (input.IsActive.HasValue)
            {
                if (input.IsActive.Value)
                {
                    spec = spec.And(new ActiveCustomFieldsSpecification());
                }
                else
                {
                    spec = spec.And(new Specification<CustomFieldDefinition>(f => !f.IsActive));
                }
            }

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                spec = spec.And(new Specification<CustomFieldDefinition>(
                    f => f.Name.Contains(input.Filter) || f.FieldKey.Contains(input.Filter)));
            }

            return spec;
        }
    }
}
