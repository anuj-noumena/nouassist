using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialModels.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Localization;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricCategories.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricDefinitions.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Permissions;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrackedMetrics.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Validation.FinancialModel;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Validation.MetricCategory;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Validation.MetricDefinition;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Validation.TrackedMetric;
using Volo.Abp;
using Volo.Abp.Application;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricCategories
{
    [Authorize(PerformanceFinancialModelingPermissions.MetricCategories.Default)]
    public class MetricCategoryAppService : ApplicationService, IMetricCategoryAppService
    {
        private readonly IRepository<MetricCategory, Guid> _metricCategoryRepository;
        private readonly IRepository<MetricDefinition, Guid> _metricDefinitionRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;

        public MetricCategoryAppService(
            IRepository<MetricCategory, Guid> metricCategoryRepository,
            IRepository<MetricDefinition, Guid> metricDefinitionRepository,
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator)
        {
            _metricCategoryRepository = metricCategoryRepository;
            _metricDefinitionRepository = metricDefinitionRepository;
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(PerformanceFinancialModelingResource);
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricCategories.View)]
        public virtual async Task<MetricCategoryDto> GetAsync(Guid id)
        {
            try
            {
                var category = await _metricCategoryRepository.GetAsync(id);
                CheckResourceAccess(category);

                var dto = ObjectMapper.Map<MetricCategory, MetricCategoryDto>(category);
                await EnrichMetricCategoryDto(dto, category);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:CategoryNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricCategories.View)]
        public virtual async Task<PagedResultDto<MetricCategoryDto>> GetListAsync(GetMetricCategoryListDto input)
        {
            try
            {
                var query = await _metricCategoryRepository.GetQueryableAsync();

                // Apply tenant filter
                query = query.Where(c => c.TenantId == _currentTenant.Id);

                // Apply filters
                query = query.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    x => x.Name.Contains(input.Filter) || x.Description.Contains(input.Filter));

                query = query.WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value);

                // Apply sorting
                if (!input.Sorting.IsNullOrWhiteSpace())
                {
                    query = query.OrderBy(input.Sorting);
                }
                else
                {
                    query = query.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name);
                }

                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);

                // Apply paging
                var entities = await AsyncExecuter.ToListAsync(query.PageBy(input));

                // Map to DTOs
                var dtos = ObjectMapper.Map<List<MetricCategory>, List<MetricCategoryDto>>(entities);

                // Enrich DTOs
                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var category = entities.First(e => e.Id == dto.Id);
                    await EnrichMetricCategoryDto(dto, category);
                }));

                return new PagedResultDto<MetricCategoryDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricCategories.Create)]
        public virtual async Task<MetricCategoryDto> CreateAsync(CreateMetricCategoryDto input)
        {
            try
            {
                // Check for duplicates
                var existingCategory = await _metricCategoryRepository.FindAsync(
                    c => c.Name == input.Name && c.TenantId == _currentTenant.Id);
                if (existingCategory != null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:DuplicateCategoryName", input.Name]);
                }

                // Create the category
                var categoryId = _guidGenerator.Create();
                var category = new MetricCategory(
                    categoryId,
                    input.Name,
                    input.Description,
                    input.Color,
                    input.DisplayOrder,
                    _currentTenant.Id
                );

                await _metricCategoryRepository.InsertAsync(category, autoSave: true);

                var dto = ObjectMapper.Map<MetricCategory, MetricCategoryDto>(category);
                await EnrichMetricCategoryDto(dto, category);

                return dto;
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

        [Authorize(PerformanceFinancialModelingPermissions.MetricCategories.Update)]
        public virtual async Task<MetricCategoryDto> UpdateAsync(Guid id, UpdateMetricCategoryDto input)
        {
            try
            {
                var category = await _metricCategoryRepository.GetAsync(id);
                CheckResourceAccess(category);

                // Check for duplicate names (excluding current category)
                var existingCategory = await _metricCategoryRepository.FindAsync(
                    c => c.Name == input.Name && c.TenantId == _currentTenant.Id && c.Id != id);
                if (existingCategory != null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:DuplicateCategoryName", input.Name]);
                }

                category.UpdateInfo(input.Name, input.Description, input.Color, input.DisplayOrder);
                await _metricCategoryRepository.UpdateAsync(category, autoSave: true);

                var dto = ObjectMapper.Map<MetricCategory, MetricCategoryDto>(category);
                await EnrichMetricCategoryDto(dto, category);

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

        [Authorize(PerformanceFinancialModelingPermissions.MetricCategories.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var category = await _metricCategoryRepository.GetAsync(id);
                CheckResourceAccess(category);

                // Check if category has definitions
                var definitionCount = await AsyncExecuter.CountAsync(
                    await _metricDefinitionRepository.GetQueryableAsync(),
                    d => d.CategoryId == id);

                if (definitionCount > 0)
                {
                    throw new UserFriendlyException(L["ErrorMessage:CategoryHasDefinitions"]);
                }

                await _metricCategoryRepository.DeleteAsync(category, autoSave: true);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:CategoryNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricCategories.Update)]
        public virtual async Task<MetricCategoryDto> ActivateAsync(Guid id)
        {
            try
            {
                var category = await _metricCategoryRepository.GetAsync(id);
                CheckResourceAccess(category);

                category.Activate();
                await _metricCategoryRepository.UpdateAsync(category, autoSave: true);

                var dto = ObjectMapper.Map<MetricCategory, MetricCategoryDto>(category);
                await EnrichMetricCategoryDto(dto, category);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:CategoryNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricCategories.Update)]
        public virtual async Task<MetricCategoryDto> DeactivateAsync(Guid id)
        {
            try
            {
                var category = await _metricCategoryRepository.GetAsync(id);
                CheckResourceAccess(category);

                category.Deactivate();
                await _metricCategoryRepository.UpdateAsync(category, autoSave: true);

                var dto = ObjectMapper.Map<MetricCategory, MetricCategoryDto>(category);
                await EnrichMetricCategoryDto(dto, category);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:CategoryNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricCategories.Update)]
        public virtual async Task ReorderCategoriesAsync(ReorderCategoriesDto input)
        {
            try
            {
                var categoryIds = input.Categories.Select(c => c.Id).ToList();
                var categories = await AsyncExecuter.ToListAsync(
                    await _metricCategoryRepository.GetQueryableAsync(),
                    c => categoryIds.Contains(c.Id) && c.TenantId == _currentTenant.Id);

                foreach (var categoryOrderItem in input.Categories)
                {
                    var category = categories.FirstOrDefault(c => c.Id == categoryOrderItem.Id);
                    if (category != null)
                    {
                        category.UpdateInfo(category.Name, category.Description, category.Color, categoryOrderItem.DisplayOrder);
                    }
                }

                await _metricCategoryRepository.UpdateManyAsync(categories, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricCategories.View)]
        public virtual async Task<List<MetricCategoryDto>> GetActiveCategoriesAsync()
        {
            try
            {
                var query = await _metricCategoryRepository.GetQueryableAsync();
                var categories = await query
                    .Where(c => c.IsActive && c.TenantId == _currentTenant.Id)
                    .OrderBy(c => c.DisplayOrder)
                    .ThenBy(c => c.Name)
                    .ToListAsync();

                var dtos = ObjectMapper.Map<List<MetricCategory>, List<MetricCategoryDto>>(categories);

                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var category = categories.First(c => c.Id == dto.Id);
                    await EnrichMetricCategoryDto(dto, category);
                }));

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.MetricCategories.View)]
        public virtual async Task<List<MetricCategoryDto>> GetOrderedCategoriesAsync()
        {
            try
            {
                var query = await _metricCategoryRepository.GetQueryableAsync();
                var categories = await query
                    .Where(c => c.TenantId == _currentTenant.Id)
                    .OrderBy(c => c.DisplayOrder)
                    .ThenBy(c => c.Name)
                    .ToListAsync();

                var dtos = ObjectMapper.Map<List<MetricCategory>, List<MetricCategoryDto>>(categories);

                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var category = categories.First(c => c.Id == dto.Id);
                    await EnrichMetricCategoryDto(dto, category);
                }));

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        // Helper methods
        protected virtual void CheckResourceAccess(MetricCategory category)
        {
            if (category.TenantId != _currentTenant.Id)
            {
                throw new EntityNotFoundException(typeof(MetricCategory), category.Id);
            }
        }

        protected virtual async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in MetricCategoryAppService: {ErrorMessage}", exception.Message);
            await Task.CompletedTask;
        }

        protected virtual async Task EnrichMetricCategoryDto(MetricCategoryDto dto, MetricCategory category)
        {
            // Get definition count
            var definitionCount = await AsyncExecuter.CountAsync(
                await _metricDefinitionRepository.GetQueryableAsync(),
                d => d.CategoryId == category.Id);

            dto.DefinitionCount = definitionCount;

            // Get metric count (simplified - would need TrackedMetric repository)
            dto.MetricCount = 0;

            await Task.CompletedTask;
        }
    }
}

// Update: src/NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Application/PerformanceFinancialModelingApplicationModule.cs
[DependsOn(
    typeof(PerformanceFinancialModelingDomainModule),
    typeof(PerformanceFinancialModelingApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpFluentValidationModule)
)]
public class PerformanceFinancialModelingApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Register AutoMapper profile
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<PerformanceFinancialModelingApplicationModule>();
            options.ValidateProfile = true;
        });

        // Register FluentValidation validators
        context.Services.AddTransient<IValidator<CreateTrackedMetricDto>, CreateTrackedMetricDtoValidator>();
        context.Services.AddTransient<IValidator<CreateFinancialModelDto>, CreateFinancialModelDtoValidator>();
        context.Services.AddTransient<IValidator<CreateMetricDefinitionDto>, CreateMetricDefinitionDtoValidator>();
        context.Services.AddTransient<IValidator<CreateMetricCategoryDto>, CreateMetricCategoryDtoValidator>();

        // Application services are automatically registered by convention
        // Additional explicit registrations if needed:
        // context.Services.AddTransient<ITrackedMetricAppService, TrackedMetricAppService>();
        // context.Services.AddTransient<IFinancialModelAppService, FinancialModelAppService>();
        // context.Services.AddTransient<IMetricDefinitionAppService, MetricDefinitionAppService>();
        // context.Services.AddTransient<IMetricCategoryAppService, MetricCategoryAppService>();
    }
}
