using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Analytics.DTOs.DataSource;
using NouAssist.Analytics.Entities;
using NouAssist.Analytics.Enums;
using NouAssist.Analytics.Permissions;
using NouAssist.Analytics.Repositories;
using NouAssist.Analytics.ValueObjects;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Interfaces.Services;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Analytics.Services
{
    [Authorize(AnalyticsPermissions.DataSources.Default)]
    public class DataSourceAppService : ApplicationService, IDataSourceAppService
    {
        private readonly IDataSourceConnector _dataSourceConnector;
        private readonly IDataSourceRepository _dataSourceRepository;
        
        public DataSourceAppService(
            IDataSourceConnector dataSourceConnector,
            IDataSourceRepository dataSourceRepository)
        {
            _dataSourceConnector = dataSourceConnector;
            _dataSourceRepository = dataSourceRepository;
        }
        
        public async Task<DataSourceDto> GetAsync(Guid id)
        {
            var dataSource = await _dataSourceRepository.GetAsync(id);
            return MapDataSourceToDto(dataSource);
        }
        
        public async Task<PagedResultDto<DataSourceDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _dataSourceRepository.CountAsync();
            
            var dataSources = await _dataSourceRepository.GetPagedListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting ?? "Name"
            );
            
            var dataSourceDtos = dataSources.Select(ds => MapDataSourceToDto(ds)).ToList();
            
            return new PagedResultDto<DataSourceDto>(totalCount, dataSourceDtos);
        }
        
        public async Task<PagedResultDto<DataSourceDto>> GetByTypeAsync(DataSourceType type, PagedResultRequestDto input)
        {
            var dataSources = await _dataSourceRepository.GetByTypeAsync(type, CurrentTenant.Id);
            
            var filteredDataSources = dataSources
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();
            
            var dataSourceDtos = filteredDataSources.Select(ds => MapDataSourceToDto(ds)).ToList();
            
            return new PagedResultDto<DataSourceDto>(dataSources.Length, dataSourceDtos);
        }
        
        [Authorize(AnalyticsPermissions.DataSources.Create)]
        public async Task<DataSourceDto> CreateAsync(CreateDataSourceDto input)
        {
            var connectionString = new DataConnectionString(input.ConnectionString.Value);
            
            var credentials = new DataSourceCredentials(
                input.Credentials?.Username,
                input.Credentials?.EncryptedPassword,
                input.Credentials?.ApiKey,
                input.Credentials?.TokenType,
                input.Credentials?.AdditionalSecrets
            );
            
            var dataSource = await _dataSourceConnector.CreateDataSourceAsync(
                input.Name,
                input.Description,
                input.Type,
                connectionString,
                credentials,
                CurrentTenant.Id
            );
            
            return MapDataSourceToDto(dataSource);
        }
        
        [Authorize(AnalyticsPermissions.DataSources.Edit)]
        public async Task<DataSourceDto> UpdateAsync(Guid id, UpdateDataSourceDto input)
        {
            var connectionString = new DataConnectionString(input.ConnectionString.Value);
            
            var credentials = new DataSourceCredentials(
                input.Credentials?.Username,
                input.Credentials?.EncryptedPassword,
                input.Credentials?.ApiKey,
                input.Credentials?.TokenType,
                input.Credentials?.AdditionalSecrets
            );
            
            var dataSource = await _dataSourceConnector.UpdateDataSourceAsync(
                id,
                input.Name,
                input.Description,
                connectionString,
                credentials
            );
            
            return MapDataSourceToDto(dataSource);
        }
        
        [Authorize(AnalyticsPermissions.DataSources.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _dataSourceRepository.DeleteAsync(id);
        }
        
        [Authorize(AnalyticsPermissions.DataSources.Edit)]
        public async Task<bool> TestConnectionAsync(Guid id)
        {
            return await _dataSourceConnector.TestConnectionAsync(id);
        }
        
        [Authorize(AnalyticsPermissions.DataSources.Edit)]
        public async Task SetActiveAsync(Guid id, bool isActive)
        {
            await _dataSourceConnector.SetDataSourceActiveAsync(id, isActive);
        }
        
        [Authorize(AnalyticsPermissions.DataSources.Edit)]
        public async Task<DataTransformationStepDto> AddTransformationStepAsync(Guid id, AddTransformationStepDto input)
        {
            var step = await _dataSourceConnector.AddTransformationStepAsync(
                id,
                input.Name,
                input.TransformationType,
                input.Configuration,
                input.Order
            );
            
            return new DataTransformationStepDto
            {
                Id = step.Id,
                Name = step.Name,
                TransformationType = step.TransformationType,
                Configuration = step.Configuration,
                Order = step.Order,
                IsEnabled = step.IsEnabled
            };
        }
        
        [Authorize(AnalyticsPermissions.DataSources.Edit)]
        public async Task RemoveTransformationStepAsync(Guid id, Guid stepId)
        {
            await _dataSourceConnector.RemoveTransformationStepAsync(id, stepId);
        }
        
        [Authorize(AnalyticsPermissions.DataSources.Edit)]
        public async Task ReorderTransformationStepsAsync(Guid id, Guid stepId, int newOrder)
        {
            await _dataSourceConnector.ReorderTransformationStepsAsync(id, stepId, newOrder);
        }
        
        [Authorize(AnalyticsPermissions.DataSources.Edit)]
        public async Task<AggregatedDataViewDto> AddAggregatedDataViewAsync(Guid id, AddAggregatedDataViewDto input)
        {
            var view = await _dataSourceConnector.AddAggregatedDataViewAsync(
                id,
                input.Name,
                input.ViewDefinition,
                input.RefreshStrategy
            );
            
            return new AggregatedDataViewDto
            {
                Id = view.Id,
                Name = view.Name,
                ViewDefinition = view.ViewDefinition,
                RefreshStrategy = view.RefreshStrategy,
                LastRefreshed = view.LastRefreshed,
                IsEnabled = view.IsEnabled
            };
        }
        
        [Authorize(AnalyticsPermissions.DataSources.Edit)]
        public async Task RemoveAggregatedDataViewAsync(Guid id, Guid viewId)
        {
            var dataSource = await _dataSourceRepository.GetAsync(id);
            dataSource.RemoveAggregatedDataView(viewId);
            await _dataSourceRepository.UpdateAsync(dataSource);
        }
        
        [Authorize(AnalyticsPermissions.DataSources.Edit)]
        public async Task RefreshAggregatedDataViewAsync(Guid id, Guid viewId)
        {
            await _dataSourceConnector.RefreshAggregatedDataViewAsync(id, viewId);
        }
        
        private DataSourceDto MapDataSourceToDto(DataSource dataSource)
        {
            return new DataSourceDto
            {
                Id = dataSource.Id,
                Name = dataSource.Name,
                Description = dataSource.Description,
                Type = dataSource.Type,
                IsActive = dataSource.IsActive,
                ConnectionString = new DataConnectionStringDto
                {
                    Value = "***SECURED***" // Don't expose actual connection string in DTOs
                },
                Credentials = new DataSourceCredentialsDto
                {
                    Username = dataSource.Credentials.Username,
                    EncryptedPassword = "***SECURED***", // Don't expose actual password
                    ApiKey = "***SECURED***", // Don't expose actual API key
                    TokenType = dataSource.Credentials.TokenType,
                    AdditionalSecrets = dataSource.Credentials.AdditionalSecrets.ToDictionary(
                        kvp => kvp.Key,
                        kvp => "***SECURED***" // Don't expose actual secrets
                    )
                },
                TransformationSteps = dataSource.TransformationSteps.Select(t => new DataTransformationStepDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    TransformationType = t.TransformationType,
                    Configuration = t.Configuration,
                    Order = t.Order,
                    IsEnabled = t.IsEnabled
                }).OrderBy(t => t.Order).ToList(),
                AggregatedDataViews = dataSource.AggregatedDataViews.Select(v => new AggregatedDataViewDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    ViewDefinition = v.ViewDefinition,
                    RefreshStrategy = v.RefreshStrategy,
                    LastRefreshed = v.LastRefreshed,
                    IsEnabled = v.IsEnabled
                }).ToList(),
                CreationTime = dataSource.CreationTime,
                CreatorId = dataSource.CreatorId,
                LastModificationTime = dataSource.LastModificationTime,
                LastModifierId = dataSource.LastModifierId
            };
        }
    }
}
