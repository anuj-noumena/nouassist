using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ConversionFunnels.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ConversionFunnels
{
    public interface IConversionFunnelAppService : IApplicationService
    {
        // Basic CRUD operations
        Task<ConversionFunnelDto> GetAsync(Guid id);
        Task<PagedResultDto<ConversionFunnelDto>> GetListAsync(GetConversionFunnelListDto input);
        Task<ConversionFunnelDto> CreateAsync(CreateConversionFunnelDto input);
        Task<ConversionFunnelDto> UpdateAsync(Guid id, UpdateConversionFunnelDto input);
        Task DeleteAsync(Guid id);
        
        // Funnel state operations
        Task<ConversionFunnelDto> SetActiveAsync(Guid id, bool isActive);
        
        // Stage operations
        Task<FunnelStageDto> AddStageAsync(Guid funnelId, CreateFunnelStageDto input);
        Task<FunnelStageDto> UpdateStageConversionRateAsync(Guid funnelId, Guid stageId, UpdateStageConversionRateDto input);
        Task<List<FunnelStageDto>> GetFunnelStagesAsync(Guid funnelId);
        
        // Analytics operations
        Task<FunnelPerformanceDto> GetFunnelPerformanceAsync(Guid funnelId);
        Task<List<ConversionFunnelDto>> GetActiveFunnelsAsync();
        Task ValidateFunnelAsync(Guid funnelId);
    }
}
