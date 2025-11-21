using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricDefinitions.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricDefinitions
{
    /// <summary>
    /// Application service for MetricDefinition management
    /// </summary>
    public interface IMetricDefinitionAppService : IApplicationService
    {
        // CRUD operations
        Task<MetricDefinitionDto> GetAsync(Guid id);
        Task<PagedResultDto<MetricDefinitionDto>> GetListAsync(GetMetricDefinitionListDto input);
        Task<MetricDefinitionDto> CreateAsync(CreateMetricDefinitionDto input);
        Task<MetricDefinitionDto> UpdateAsync(Guid id, UpdateMetricDefinitionDto input);
        Task DeleteAsync(Guid id);

        // Status management
        Task<MetricDefinitionDto> ActivateAsync(Guid id);
        Task<MetricDefinitionDto> DeactivateAsync(Guid id);

        // Lookups
        Task<List<MetricDefinitionDto>> GetByTypeAsync(MetricType type);
        Task<List<MetricDefinitionDto>> GetActiveDefinitionsAsync();
        Task<List<MetricDefinitionDto>> GetByCategoryAsync(Guid categoryId);

        // Validation
        Task<bool> ValidateCalculationRuleAsync(CalculationRuleDto calculationRule);
        Task<List<string>> GetAvailableMetricsAsync();
    }
}
