using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.LearningDevelopment.Learning.LearningSessions.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.LearningDevelopment.Learning.LearningSessions
{
    public interface ILearningSessionAppService : IApplicationService
    {
        Task<LearningSessionDto> GetAsync(Guid id);
        Task<LearningSessionDto> GetByTopicIdAsync(Guid topicId);
        Task<List<LearningSessionDto>> GetActiveSessionsAsync();
        Task<List<LearningSessionDto>> GetPausedSessionsAsync();
        Task<LearningSessionDto> StartSessionAsync(Guid topicId);
        Task<LearningSessionDto> AdvancePhaseAsync(Guid id, AdvancePhaseDto input);
        Task<LearningSessionDto> PauseSessionAsync(Guid id);
        Task<LearningSessionDto> ResumeSessionAsync(Guid id);
        Task RecordPreLearningQuestionsAsync(Guid id, RecordPreLearningQuestionsDto input);
        Task RecordSectionReflectionAsync(Guid id, RecordSectionReflectionDto input);
        Task RecordFeynmanSummaryAsync(Guid id, RecordFeynmanSummaryDto input);
        Task RecordBloomAssessmentAsync(Guid id, RecordBloomAssessmentDto input);
        Task CompleteSpacedCycleAsync(Guid id, CompleteSpacedCycleDto input);
        Task RecordMasteryValidationAsync(Guid id, RecordMasteryValidationDto input);
    }
}
