using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Localization;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricDefinitions.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Permissions;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricDefinitions
{
    [Authorize(PerformanceFinancialModelingPermissions.MetricDefinitions.Default)]
    public class MetricDefinitionAppService : ApplicationService, IMetricDefinitionAppService
    {
        private readonly IRepository<MetricDefinition, Guid> _metricDefinitionRepository;
        private readonly IRepository<MetricCategory, Guid> _metricCategoryRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;

        public MetricDefinitionAppService(
            IRepository<MetricDefinition, Guid> metricDefinitionRepository,
            IRepository<MetricCategory, Guid> metricCategoryRepository,
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator)
        {
            _metricDefinitionRepository = metricDefinitionRepository;
            _metricCategoryRepository = metricCategoryRepository;
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(PerformanceFinancialModelingResource);
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricDefinitions.View)]
        public virtual async Task<MetricDefinitionDto> GetAsync(Guid id)
        {
            try
            {
                var definition = await _metricDefinitionRepository.GetAsync(id);
                CheckResourceAccess(definition);

                var dto = ObjectMapper.Map<MetricDefinition, MetricDefinitionDto>(definition);
                await EnrichMetricDefinitionDto(dto, definition);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:DefinitionNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricDefinitions.View)]
        public virtual async Task<PagedResultDto<MetricDefinitionDto>> GetListAsync(GetMetricDefinitionListDto input)
        {
            try
            {
                var query = await _metricDefinitionRepository.GetQueryableAsync();

                // Apply tenant filter
                query = query.Where(d => d.TenantId == _currentTenant.Id);

                // Apply filters
                query = query.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    x => x.Name.Contains(input.Filter) || x.Description.Contains(input.Filter));

                query = AbpQueryableExtensions.WhereIf(query, input.Type.HasValue, x => x.Type == input.Type.Value);
                query = query.WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value);
                query = AbpQueryableExtensions.WhereIf(query, input.CategoryId.HasValue, x => x.CategoryId == input.CategoryId.Value);
                query = AbpQueryableExtensions.WhereIf(query, input.IsAutomated.HasValue, x => x.CalculationRule.IsAutomated == input.IsAutomated.Value);

                // Apply sorting
                if (!input.Sorting.IsNullOrWhiteSpace())
                {
                    query = query.OrderBy(input.Sorting);
                }
                else
                {
                    query = query.OrderBy(x => x.Name);
                }

                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);

                // Apply paging
                var entities = await AsyncExecuter.ToListAsync(query.PageBy(input));

                // Map to DTOs
                var dtos = ObjectMapper.Map<List<MetricDefinition>, List<MetricDefinitionDto>>(entities);

                // Enrich DTOs
                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var definition = entities.First(e => e.Id == dto.Id);
                    await EnrichMetricDefinitionDto(dto, definition);
                }));

                return new PagedResultDto<MetricDefinitionDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricDefinitions.Create)]
        public virtual async Task<MetricDefinitionDto> CreateAsync(CreateMetricDefinitionDto input)
        {
            try
            {
                // Check for duplicates
                var existingDefinition = await _metricDefinitionRepository.FindAsync(
                    d => d.Name == input.Name && d.TenantId == _currentTenant.Id);
                if (existingDefinition != null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:DuplicateDefinitionName", input.Name]);
                }

                // Validate category if provided
                if (input.CategoryId.HasValue)
                {
                    var category = await _metricCategoryRepository.GetAsync(input.CategoryId.Value);
                    if (!category.IsActive)
                    {
                        throw new UserFriendlyException(L["ErrorMessage:InactiveCategory"]);
                    }
                }

                // Create calculation rule
                var calculationRule = ObjectMapper.Map<CalculationRuleDto, CalculationRule>(input.CalculationRule);

                // Create the definition
                var definitionId = _guidGenerator.Create();
                var definition = new MetricDefinition(
                    definitionId,
                    input.Name,
                    input.Description,
                    input.Type,
                    input.DataType,
                    calculationRule,
                    input.Unit,
                    input.CategoryId,
                    _currentTenant.Id
                );

                await _metricDefinitionRepository.InsertAsync(definition, autoSave: true);

                var dto = ObjectMapper.Map<MetricDefinition, MetricDefinitionDto>(definition);
                await EnrichMetricDefinitionDto(dto, definition);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:CategoryNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricDefinitions.Update)]
        public virtual async Task<MetricDefinitionDto> UpdateAsync(Guid id, UpdateMetricDefinitionDto input)
        {
            try
            {
                var definition = await _metricDefinitionRepository.GetAsync(id);
                CheckResourceAccess(definition);

                // Check for duplicate names (excluding current definition)
                var existingDefinition = await _metricDefinitionRepository.FindAsync(
                    d => d.Name == input.Name && d.TenantId == _currentTenant.Id && d.Id != id);
                if (existingDefinition != null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:DuplicateDefinitionName", input.Name]);
                }

                // Validate category if provided
                if (input.CategoryId.HasValue)
                {
                    var category = await _metricCategoryRepository.GetAsync(input.CategoryId.Value);
                    if (!category.IsActive)
                    {
                        throw new UserFriendlyException(L["ErrorMessage:InactiveCategory"]);
                    }
                }

                // Update calculation rule
                var calculationRule = ObjectMapper.Map<CalculationRuleDto, CalculationRule>(input.CalculationRule);

                definition.UpdateDefinition(input.Name, input.Description, calculationRule, input.Unit);
                definition.SetCategory(input.CategoryId);

                await _metricDefinitionRepository.UpdateAsync(definition, autoSave: true);

                var dto = ObjectMapper.Map<MetricDefinition, MetricDefinitionDto>(definition);
                await EnrichMetricDefinitionDto(dto, definition);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:DefinitionNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricDefinitions.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var definition = await _metricDefinitionRepository.GetAsync(id);
                CheckResourceAccess(definition);

                await _metricDefinitionRepository.DeleteAsync(definition, autoSave: true);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:DefinitionNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricDefinitions.Update)]
        public virtual async Task<MetricDefinitionDto> ActivateAsync(Guid id)
        {
            try
            {
                var definition = await _metricDefinitionRepository.GetAsync(id);
                CheckResourceAccess(definition);

                definition.Activate();
                await _metricDefinitionRepository.UpdateAsync(definition, autoSave: true);

                var dto = ObjectMapper.Map<MetricDefinition, MetricDefinitionDto>(definition);
                await EnrichMetricDefinitionDto(dto, definition);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:DefinitionNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricDefinitions.Update)]
        public virtual async Task<MetricDefinitionDto> DeactivateAsync(Guid id)
        {
            try
            {
                var definition = await _metricDefinitionRepository.GetAsync(id);
                CheckResourceAccess(definition);

                definition.Deactivate();
                await _metricDefinitionRepository.UpdateAsync(definition, autoSave: true);

                var dto = ObjectMapper.Map<MetricDefinition, MetricDefinitionDto>(definition);
                await EnrichMetricDefinitionDto(dto, definition);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:DefinitionNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricDefinitions.View)]
        public virtual async Task<List<MetricDefinitionDto>> GetByTypeAsync(MetricType type)
        {
            try
            {
                var query = await _metricDefinitionRepository.GetQueryableAsync();
                var definitions = await query
                    .Where(d => d.Type == type && d.TenantId == _currentTenant.Id && d.IsActive)
                    .OrderBy(d => d.Name)
                    .ToListAsync();

                var dtos = ObjectMapper.Map<List<MetricDefinition>, List<MetricDefinitionDto>>(definitions);

                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var definition = definitions.First(d => d.Id == dto.Id);
                    await EnrichMetricDefinitionDto(dto, definition);
                }));

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricDefinitions.View)]
        public virtual async Task<List<MetricDefinitionDto>> GetActiveDefinitionsAsync()
        {
            try
            {
                var query = await _metricDefinitionRepository.GetQueryableAsync();
                var definitions = await query
                    .Where(d => d.IsActive && d.TenantId == _currentTenant.Id)
                    .OrderBy(d => d.Name)
                    .ToListAsync();

                var dtos = ObjectMapper.Map<List<MetricDefinition>, List<MetricDefinitionDto>>(definitions);

                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var definition = definitions.First(d => d.Id == dto.Id);
                    await EnrichMetricDefinitionDto(dto, definition);
                }));

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricDefinitions.View)]
        public virtual async Task<List<MetricDefinitionDto>> GetByCategoryAsync(Guid categoryId)
        {
            try
            {
                var query = await _metricDefinitionRepository.GetQueryableAsync();
                var definitions = await query
                    .Where(d => d.CategoryId == categoryId && d.TenantId == _currentTenant.Id && d.IsActive)
                    .OrderBy(d => d.Name)
                    .ToListAsync();

                var dtos = ObjectMapper.Map<List<MetricDefinition>, List<MetricDefinitionDto>>(definitions);

                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var definition = definitions.First(d => d.Id == dto.Id);
                    await EnrichMetricDefinitionDto(dto, definition);
                }));

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricDefinitions.View)]
        public virtual async Task<bool> ValidateCalculationRuleAsync(CalculationRuleDto calculationRule)
        {
            try
            {
                // Basic validation
                if (string.IsNullOrWhiteSpace(calculationRule.Formula))
                    return false;

                // TODO: Implement more sophisticated validation
                // - Parse formula syntax
                // - Check for required metrics existence
                // - Validate mathematical operations

                return true;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                return false;
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricDefinitions.View)]
        public virtual async Task<List<string>> GetAvailableMetricsAsync()
        {
            try
            {
                var query = await _metricDefinitionRepository.GetQueryableAsync();
                var metricNames = await query
                    .Where(d => d.IsActive && d.TenantId == _currentTenant.Id)
                    .Select(d => d.Name)
                    .ToListAsync();

                return metricNames;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        // Helper methods
        protected virtual void CheckResourceAccess(MetricDefinition definition)
        {
            if (definition.TenantId != _currentTenant.Id)
            {
                throw new EntityNotFoundException(typeof(MetricDefinition), definition.Id);
            }
        }

        protected virtual async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in MetricDefinitionAppService: {ErrorMessage}", exception.Message);
            await Task.CompletedTask;
        }

        protected virtual async Task EnrichMetricDefinitionDto(MetricDefinitionDto dto, MetricDefinition definition)
        {
            // Get category name
            if (definition.CategoryId.HasValue)
            {
                try
                {
                    var category = await _metricCategoryRepository.FindAsync(definition.CategoryId.Value);
                    dto.CategoryName = category?.Name;
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex, "Failed to get category name for definition {DefinitionId}", definition.Id);
                }
            }

            // Get usage count (simplified - would need to query TrackedMetric repository)
            dto.UsageCount = 0;

            await Task.CompletedTask;
        }
    }
}
