using System;
using System.Threading.Tasks;
using NouAssist.SkillDevelopment.DevelopmentSessions.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.SkillDevelopment.DevelopmentSessions
{
    public interface ISkillDevelopmentSessionAppService : IApplicationService
    {
        Task<SkillDevelopmentSessionDto> GetAsync(Guid id);
        Task<SkillDevelopmentSessionDto> GetBySkillIdAsync(Guid skillId);
        Task<PagedResultDto<SkillDevelopmentSessionDto>> GetListAsync(GetSessionListDto input);
        Task<SkillDevelopmentSessionDto> StartAsync(StartDevelopmentSessionDto input);
        Task<SkillDevelopmentSessionDto> RecordAssessmentAsync(Guid sessionId, RecordAssessmentDto input);
        Task<SkillDevelopmentSessionDto> AddPracticePlanAsync(Guid sessionId, AddPracticePlanDto input);
        Task<SkillDevelopmentSessionDto> AddPracticeLogAsync(Guid sessionId, AddPracticeLogDto input);
        Task<SkillDevelopmentSessionDto> AddReviewFeedbackAsync(Guid sessionId, AddReviewFeedbackDto input);
        Task<SkillDevelopmentSessionDto> AddApplicationExampleAsync(Guid sessionId, AddApplicationExampleDto input);
        Task<SkillDevelopmentSessionDto> RecordMasteryValidationAsync(Guid sessionId, RecordMasteryValidationDto input);
        Task<SkillDevelopmentSessionDto> CompleteCurrentStepAsync(Guid sessionId);
        Task<SkillDevelopmentSessionDto> AdvanceToNextStepAsync(Guid sessionId);
        Task<SkillDevelopmentSessionDto> PauseAsync(Guid sessionId);
        Task<SkillDevelopmentSessionDto> ResumeAsync(Guid sessionId);
        Task<SkillDevelopmentSessionDto> StartReviewCycleAsync(Guid sessionId);
    }
}
