using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.DataSources.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.DataSources
{
    /// <summary>
    /// Application service for data source management
    /// </summary>
    public interface IDataSourceAppService : IApplicationService
    {
        /// <summary>
        /// Gets a data source by ID
        /// </summary>
        Task<DataSourceDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a paginated list of data sources
        /// </summary>
        Task<PagedResultDto<DataSourceDto>> GetListAsync(GetDataSourceListDto input);
        
        /// <summary>
        /// Creates a new data source
        /// </summary>
        Task<DataSourceDto> CreateAsync(CreateDataSourceDto input);
        
        /// <summary>
        /// Updates an existing data source
        /// </summary>
        Task<DataSourceDto> UpdateAsync(Guid id, UpdateDataSourceDto input);
        
        /// <summary>
        /// Deletes a data source
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Updates credentials for a data source
        /// </summary>
        Task UpdateCredentialsAsync(Guid id, UpdateCredentialsDto input);
        
        /// <summary>
        /// Refreshes a data source
        /// </summary>
        Task RefreshDataSourceAsync(Guid id);
        
        /// <summary>
        /// Adds a transformation step to a data source
        /// </summary>
        Task<DataTransformationStepDto> AddTransformationStepAsync(Guid dataSourceId, AddTransformationStepDto input);
        
        /// <summary>
        /// Updates a transformation step on a data source
        /// </summary>
        Task<DataTransformationStepDto> UpdateTransformationStepAsync(Guid dataSourceId, Guid stepId, UpdateTransformationStepDto input);
        
        /// <summary>
        /// Removes a transformation step from a data source
        /// </summary>
        Task DeleteTransformationStepAsync(Guid dataSourceId, Guid stepId);
        
        /// <summary>
        /// Creates a new aggregated data view
        /// </summary>
        Task<AggregatedDataViewDto> CreateAggregatedDataViewAsync(CreateAggregatedDataViewDto input);
        
        /// <summary>
        /// Gets an aggregated data view by ID
        /// </summary>
        Task<AggregatedDataViewDto> GetAggregatedDataViewAsync(Guid id);
        
        /// <summary>
        /// Gets a paginated list of aggregated data views
        /// </summary>
        Task<PagedResultDto<AggregatedDataViewDto>> GetAggregatedDataViewListAsync(GetAggregatedDataViewListDto input);
        
        /// <summary>
        /// Updates an aggregated data view
        /// </summary>
        Task<AggregatedDataViewDto> UpdateAggregatedDataViewAsync(Guid id, UpdateAggregatedDataViewDto input);
        
        /// <summary>
        /// Deletes an aggregated data view
        /// </summary>
        Task DeleteAggregatedDataViewAsync(Guid id);
        
        /// <summary>
        /// Refreshes an aggregated data view
        /// </summary>
        Task RefreshAggregatedDataViewAsync(Guid id);
    }
}
