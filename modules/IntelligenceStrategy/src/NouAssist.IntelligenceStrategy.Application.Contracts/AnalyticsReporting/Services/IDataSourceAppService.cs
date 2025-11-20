using System;
using System.Threading.Tasks;
using NouAssist.Analytics.DTOs.DataSource;
using NouAssist.Analytics.Enums;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Analytics.Services
{
    /// <summary>
    /// Application service interface for data source operations
    /// </summary>
    public interface IDataSourceAppService : IApplicationService
    {
        Task<DataSourceDto> GetAsync(Guid id);
        
        Task<PagedResultDto<DataSourceDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        
        Task<PagedResultDto<DataSourceDto>> GetByTypeAsync(DataSourceType type, PagedResultRequestDto input);
        
        Task<DataSourceDto> CreateAsync(CreateDataSourceDto input);
        
        Task<DataSourceDto> UpdateAsync(Guid id, UpdateDataSourceDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<bool> TestConnectionAsync(Guid id);
        
        Task SetActiveAsync(Guid id, bool isActive);
        
        Task<DataTransformationStepDto> AddTransformationStepAsync(Guid id, AddTransformationStepDto input);
        
        Task RemoveTransformationStepAsync(Guid id, Guid stepId);
        
        Task ReorderTransformationStepsAsync(Guid id, Guid stepId, int newOrder);
        
        Task<AggregatedDataViewDto> AddAggregatedDataViewAsync(Guid id, AddAggregatedDataViewDto input);
        
        Task RemoveAggregatedDataViewAsync(Guid id, Guid viewId);
        
        Task RefreshAggregatedDataViewAsync(Guid id, Guid viewId);
    }
}
