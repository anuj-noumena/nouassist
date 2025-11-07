using System;
using System.Threading.Tasks;
using NouAssist.WorkflowOrchestration.GTDSystem.Dtos;
using NouAssist.WorkflowOrchestration.GTDSystem.Enums;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkflowOrchestration.GTDSystem.GTD
{
    public interface IGtdHorizonAppService : IApplicationService
    {
        Task<GtdHorizonDto> GetAsync(Guid id);
        Task<GtdHorizonDto?> GetByLevelAsync(GtdHorizonLevel level);
        Task<ListResultDto<GtdHorizonDto>> GetMyHorizonsAsync();
        Task<ListResultDto<GtdHorizonDto>> GetHorizonsNeedingReviewAsync();
        Task<GtdHorizonDto> CreateAsync(CreateGtdHorizonDto input);
        Task<GtdHorizonDto> UpdateAsync(Guid id, UpdateGtdHorizonDto input);
        Task DeleteAsync(Guid id);
        Task<GtdHorizonDto> MarkAsReviewedAsync(Guid id);
        Task<GtdHorizonDto> PublishAsync(Guid id);
        Task<GtdHorizonDto> LinkGoalAsync(Guid id, Guid goalId);
        Task<GtdHorizonDto> LinkAreaAsync(Guid id, Guid areaId);
        Task<GtdHorizonDto> LinkProjectAsync(Guid id, Guid projectId);
        Task<GtdHorizonDto> AddCoreValueAsync(Guid id, string value);
        Task<GtdHorizonDto> AddKeyThemeAsync(Guid id, string theme);
    }

    public interface IGtdHorizonAlignmentAppService : IApplicationService
    {
        Task<GtdHorizonAlignmentDto> CreateAlignmentAsync(CreateAlignmentDto input);
        Task<GtdHorizonAlignmentDto> UpdateStrengthAsync(Guid id, int strength);
        Task<GtdHorizonAlignmentDto> ValidateAlignmentAsync(Guid id, ValidateAlignmentDto input);
        Task<ListResultDto<GtdHorizonAlignmentDto>> GetWeakAlignmentsAsync();
        Task DeleteAlignmentAsync(Guid id);
    }
}
