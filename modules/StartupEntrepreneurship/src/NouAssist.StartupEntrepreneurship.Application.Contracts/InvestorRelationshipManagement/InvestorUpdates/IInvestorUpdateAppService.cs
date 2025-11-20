using System;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates
{
    /// <summary>
    /// Application service interface for investor update management
    /// </summary>
    public interface IInvestorUpdateAppService : IApplicationService
    {
        // CRUD Operations
        Task<InvestorUpdateDto> GetAsync(Guid id);
        Task<InvestorUpdateDto> GetWithDetailsAsync(Guid id);
        Task<PagedResultDto<InvestorUpdateDto>> GetListAsync(GetInvestorUpdateListDto input);
        Task<InvestorUpdateDto> CreateAsync(CreateInvestorUpdateDto input);
        Task<InvestorUpdateDto> UpdateAsync(Guid id, UpdateInvestorUpdateDto input);
        Task DeleteAsync(Guid id);

        // Section Management
        Task<UpdateSectionDto> AddSectionAsync(Guid id, CreateUpdateSectionDto input);
        Task<UpdateSectionDto> UpdateSectionAsync(Guid id, Guid sectionId, UpdateUpdateSectionDto input);
        Task DeleteSectionAsync(Guid id, Guid sectionId);

        // Metric Management
        Task<MetricSnapshotDto> AddMetricAsync(Guid id, CreateMetricSnapshotDto input);
        Task DeleteMetricAsync(Guid id, Guid metricId);

        // Update Scheduling and Sending
        Task ScheduleAsync(Guid id, ScheduleUpdateDto input);
        Task<InvestorUpdateDto> SendAsync(Guid id, SendUpdateDto input);
        Task<PagedResultDto<InvestorUpdateDto>> GetScheduledUpdatesAsync(PagedAndSortedResultRequestDto input);

        // Engagement Tracking
        Task TrackEngagementAsync(Guid id, TrackEngagementDto input);
        Task<PagedResultDto<EngagementRecordDto>> GetEngagementRecordsAsync(Guid id, PagedAndSortedResultRequestDto input);
        Task<PagedResultDto<DistributionLogDto>> GetDistributionLogsAsync(Guid id, PagedAndSortedResultRequestDto input);

        // Analytics
        Task<decimal> GetEngagementRateAsync(Guid id);
        Task<int> GetDeliveryCountAsync(Guid id);
        Task<int> GetReadCountAsync(Guid id);
    }
}
