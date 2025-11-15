using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Learning.Aggregates;
using NouAssist.Learning.Data;
using NouAssist.Learning.LearningSessions.Dtos;
using NouAssist.Learning.Permissions;
using NouAssist.Learning.Services;
using NouAssist.Learning.ValueObjects;
using Volo.Abp.Application.Services;

namespace NouAssist.Learning.LearningSessions
{
    [Authorize(LearningPermissions.LearningSessions.Default)]
    public class LearningSessionAppService : ApplicationService, ILearningSessionAppService
    {
        private readonly ILearningSessionRepository _sessionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly LearningWorkflowService _workflowService;
        private readonly LearningProgressService _progressService;

        public LearningSessionAppService(
            ILearningSessionRepository sessionRepository,
            ITopicRepository topicRepository,
            LearningWorkflowService workflowService,
            LearningProgressService progressService)
        {
            _sessionRepository = sessionRepository;
            _topicRepository = topicRepository;
            _workflowService = workflowService;
            _progressService = progressService;
        }

        public async Task<LearningSessionDto> GetAsync(Guid id)
        {
            var session = await _sessionRepository.GetAsync(id, includeDetails: true);
            return ObjectMapper.Map<LearningSession, LearningSessionDto>(session);
        }

        public async Task<LearningSessionDto> GetByTopicIdAsync(Guid topicId)
        {
            var session = await _sessionRepository.GetByTopicIdAsync(topicId);
            return ObjectMapper.Map<LearningSession, LearningSessionDto>(session);
        }

        public async Task<List<LearningSessionDto>> GetActiveSessionsAsync()
        {
            var sessions = await _sessionRepository.GetActiveSessionsAsync(CurrentUser.GetId());
            return ObjectMapper.Map<List<LearningSession>, List<LearningSessionDto>>(sessions);
        }

        public async Task<List<LearningSessionDto>> GetPausedSessionsAsync()
        {
            var sessions = await _sessionRepository.GetPausedSessionsAsync(CurrentUser.GetId());
            return ObjectMapper.Map<List<LearningSession>, List<LearningSessionDto>>(sessions);
        }

        [Authorize(LearningPermissions.LearningSessions.Start)]
        public async Task<LearningSessionDto> StartSessionAsync(Guid topicId)
        {
            var topic = await _topicRepository.GetAsync(topicId);

            // Check if session already exists
            var existingSession = await _sessionRepository.GetByTopicIdAsync(topicId);
            if (existingSession != null)
            {
                throw new Volo.Abp.BusinessException("Learning:SessionAlreadyExists",
                    "A learning session already exists for this topic.");
            }

            var session = _workflowService.StartNewSession(topic, CurrentUser.GetId());

            await _sessionRepository.InsertAsync(session);
            await _topicRepository.UpdateAsync(topic);

            return ObjectMapper.Map<LearningSession, LearningSessionDto>(session);
        }

        [Authorize(LearningPermissions.LearningSessions.Advance)]
        public async Task<LearningSessionDto> AdvancePhaseAsync(Guid id, AdvancePhaseDto input)
        {
            var session = await _sessionRepository.GetAsync(id, includeDetails: true);
            var topic = await _topicRepository.GetAsync(session.TopicId);

            _workflowService.AdvancePhase(session, topic, input.TimeSpentMs);

            await _sessionRepository.UpdateAsync(session);
            await _topicRepository.UpdateAsync(topic);

            return ObjectMapper.Map<LearningSession, LearningSessionDto>(session);
        }

        [Authorize(LearningPermissions.LearningSessions.Pause)]
        public async Task<LearningSessionDto> PauseSessionAsync(Guid id)
        {
            var session = await _sessionRepository.GetAsync(id);
            session.PauseSession();
            await _sessionRepository.UpdateAsync(session);
            return ObjectMapper.Map<LearningSession, LearningSessionDto>(session);
        }

        [Authorize(LearningPermissions.LearningSessions.Resume)]
        public async Task<LearningSessionDto> ResumeSessionAsync(Guid id)
        {
            var session = await _sessionRepository.GetAsync(id);
            session.ResumeSession();
            await _sessionRepository.UpdateAsync(session);
            return ObjectMapper.Map<LearningSession, LearningSessionDto>(session);
        }

        [Authorize(LearningPermissions.LearningSessions.RecordData)]
        public async Task RecordPreLearningQuestionsAsync(
            Guid id, RecordPreLearningQuestionsDto input)
        {
            var session = await _sessionRepository.GetAsync(id, includeDetails: true);
            _workflowService.RecordPreLearningQuestions(session, input.Questions);
            await _sessionRepository.UpdateAsync(session);
        }

        [Authorize(LearningPermissions.LearningSessions.RecordData)]
        public async Task RecordSectionReflectionAsync(
            Guid id, RecordSectionReflectionDto input)
        {
            var session = await _sessionRepository.GetAsync(id, includeDetails: true);
            _workflowService.RecordSectionReflection(session, input.SectionKey, input.Reflection);
            await _sessionRepository.UpdateAsync(session);
        }

        [Authorize(LearningPermissions.LearningSessions.RecordData)]
        public async Task RecordFeynmanSummaryAsync(
            Guid id, RecordFeynmanSummaryDto input)
        {
            var session = await _sessionRepository.GetAsync(id, includeDetails: true);
            _workflowService.RecordFeynmanSummary(session, input.Summary);
            await _sessionRepository.UpdateAsync(session);
        }

        [Authorize(LearningPermissions.LearningSessions.RecordData)]
        public async Task RecordBloomAssessmentAsync(
            Guid id, RecordBloomAssessmentDto input)
        {
            var session = await _sessionRepository.GetAsync(id, includeDetails: true);
            
            var assessment = new BloomAssessmentResult(
                input.RememberScore,
                input.UnderstandScore,
                input.ApplyScore,
                input.AnalyzeScore,
                input.EvaluateScore,
                input.CreateScore
            );

            _workflowService.RecordBloomAssessment(session, assessment);
            await _sessionRepository.UpdateAsync(session);
        }

        [Authorize(LearningPermissions.LearningSessions.RecordData)]
        public async Task CompleteSpacedCycleAsync(
            Guid id, CompleteSpacedCycleDto input)
        {
            var session = await _sessionRepository.GetAsync(id, includeDetails: true);

            _workflowService.CompleteSpacedRepetitionCycle(
                session,
                input.CycleNumber,
                input.RecallAccuracy,
                input.AverageConfidence,
                input.CompletionRate,
                input.Improvements,
                input.Reflection
            );

            await _sessionRepository.UpdateAsync(session);
        }

        [Authorize(LearningPermissions.LearningSessions.RecordData)]
        public async Task RecordMasteryValidationAsync(
            Guid id, RecordMasteryValidationDto input)
        {
            var session = await _sessionRepository.GetAsync(id, includeDetails: true);
            _workflowService.RecordMasteryValidation(session, input.ValidationData);
            await _sessionRepository.UpdateAsync(session);
        }
    }
}
