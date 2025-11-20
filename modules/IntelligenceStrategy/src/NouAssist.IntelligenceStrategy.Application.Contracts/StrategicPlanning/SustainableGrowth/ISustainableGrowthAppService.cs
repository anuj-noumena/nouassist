using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using NouAssist.IntelligenceStrategy.StrategicPlanning.SustainableGrowth.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.SustainableGrowth
{
    /// <summary>
    /// Application service interface for Sustainable Growth Framework management
    /// </summary>
    public interface ISustainableGrowthAppService : IApplicationService
    {
        Task<SustainableGrowthFrameworkDto> GetAsync(Guid id);
        Task<PagedResultDto<SustainableGrowthFrameworkDto>> GetListAsync(PagedResultRequestDto input);
        Task<SustainableGrowthFrameworkDto> CreateAsync(CreateSustainableGrowthFrameworkDto input);
        Task<SustainableGrowthFrameworkDto> UpdateAsync(Guid id, CreateSustainableGrowthFrameworkDto input);
        Task DeleteAsync(Guid id);

        // ESG Goal Management
        Task<SustainableGrowthFrameworkDto> AddEsgGoalAsync(Guid frameworkId, AddEsgGoalDto input);
        Task<SustainableGrowthFrameworkDto> RecordEsgMeasurementAsync(Guid frameworkId, Guid esgGoalId, RecordEsgMeasurementDto input);

        // Framework Operations
        Task<SustainableGrowthFrameworkDto> ActivateAsync(Guid id);
        Task<SustainableGrowthFrameworkDto> DeactivateAsync(Guid id);

        // Reporting
        Task<ListResultDto<SustainableGrowthFrameworkDto>> GetActiveFrameworksAsync();
        Task<ListResultDto<EsgGoalRecordDto>> GetGoalsBehindScheduleAsync(Guid frameworkId);
        Task<ListResultDto<EsgGoalRecordDto>> GetGoalsByCategoryAsync(Guid frameworkId, EsgCategory category);
    }
}
