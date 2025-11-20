using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricCategories.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricCategories
{
    /// <summary>
    /// Application service for MetricCategory management
    /// </summary>
    public interface IMetricCategoryAppService : IApplicationService
    {
        // CRUD operations
        Task<MetricCategoryDto> GetAsync(Guid id);
        Task<PagedResultDto<MetricCategoryDto>> GetListAsync(GetMetricCategoryListDto input);
        Task<MetricCategoryDto> CreateAsync(CreateMetricCategoryDto input);
        Task<MetricCategoryDto> UpdateAsync(Guid id, UpdateMetricCategoryDto input);
        Task DeleteAsync(Guid id);

        // Status management
        Task<MetricCategoryDto> ActivateAsync(Guid id);
        Task<MetricCategoryDto> DeactivateAsync(Guid id);

        // Ordering
        Task ReorderCategoriesAsync(ReorderCategoriesDto input);

        // Lookups
        Task<List<MetricCategoryDto>> GetActiveCategoriesAsync();
        Task<List<MetricCategoryDto>> GetOrderedCategoriesAsync();
    }
}
