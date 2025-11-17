using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.LearningDevelopment.SkillDevelopment.Data;
using NouAssist.LearningDevelopment.SkillDevelopment.DevelopmentSessions.Dtos;
using NouAssist.LearningDevelopment.SkillDevelopment.Entities;
using NouAssist.LearningDevelopment.SkillDevelopment.Permissions;
using NouAssist.LearningDevelopment.SkillDevelopment.Services;
using NouAssist.LearningDevelopment.SkillDevelopment.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.LearningDevelopment.SkillDevelopment.DevelopmentSessions
{
    [Authorize(SkillDevelopmentPermissions.DevelopmentSessions.Default)]
    public class SkillDevelopmentSessionAppService : ApplicationService, ISkillDevelopmentSessionAppService
    {
        private readonly ISkillDevelopmentSessionRepository _sessionRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly SkillProgressionService _progressionService;
        private readonly SkillValidationService _validationService;

        public SkillDevelopmentSessionAppService(
            ISkillDevelopmentSessionRepository sessionRepository,
            ISkillRepository skillRepository,
            SkillProgressionService progressionService,
            SkillValidationService validationService)
        {
            _sessionRepository = sessionRepository;
            _skillRepository = skillRepository;
            _progressionService = progressionService;
            _validationService = validationService;
        }

        public async Task<SkillDevelopmentSessionDto> GetAsync(Guid id)
        {
            var session = await _sessionRepository.GetAsync(id);

            // Check ownership
            if (session.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            return await MapToDto(session);
        }

        public async Task<SkillDevelopmentSessionDto> GetBySkillIdAsync(Guid skillId)
        {
            var session = await _sessionRepository.GetBySkillIdAsync(skillId);
            if (session == null)
            {
                throw new BusinessException("SkillDevelopment:SessionNotFound");
            }

            // Check ownership
            if (session.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            return await MapToDto(session);
        }

        public async Task<PagedResultDto<SkillDevelopmentSessionDto>> GetListAsync(GetSessionListDto input)
        {
            var queryable = await _sessionRepository.GetQueryableAsync();

            // Filter by current user
            queryable = queryable.Where(s => s.UserId == CurrentUser.Id.Value);

            // Apply filters
            if (input.SkillId.HasValue)
            {
                queryable = queryable.Where(s => s.SkillId == input.SkillId.Value);
            }

            if (input.Completed.HasValue)
            {
                queryable = queryable.Where(s => s.Completed == input.Completed.Value);
            }

            if (input.Paused.HasValue)
            {
                queryable = queryable.Where(s => s.Paused == input.Paused.Value);
            }

            if (input.CurrentStep.HasValue)
            {
                queryable = queryable.Where(s => s.CurrentStep == input.CurrentStep.Value);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(s => s.LastActivity)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var sessions = await AsyncExecuter.ToListAsync(queryable);

            var dtos = new List<SkillDevelopmentSessionDto>();
            foreach (var session in sessions)
            {
                dtos.Add(await MapToDto(session));
            }

            return new PagedResultDto<SkillDevelopmentSessionDto>(totalCount, dtos);
        }

        [Authorize(SkillDevelopmentPermissions.DevelopmentSessions.Start)]
        public async Task<SkillDevelopmentSessionDto> StartAsync(StartDevelopmentSessionDto input)
        {
            // Load skill and validate
            var skill = await _skillRepository.GetAsync(input.SkillId);
            
            if (skill.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            // Load prerequisites
            var prerequisites = new List<Skill>();
            if (skill.Prerequisites.Any())
            {
                foreach (var prereqId in skill.Prerequisites)
                {
                    prerequisites.Add(await _skillRepository.GetAsync(prereqId));
                }
            }

            // Validate can start
            if (!_validationService.CanStartDevelopmentSession(skill, prerequisites))
            {
                throw new BusinessException("SkillDevelopment:CannotStartSession")
                    .WithData("SkillId", skill.Id);
            }

            // Create session
            var session = new SkillDevelopmentSession(
                GuidGenerator.Create(),
                CurrentUser.Id.Value,
                skill.Id,
                CurrentTenant.Id);

            // Update skill
            skill.StartDevelopment(session.Id);

            await _sessionRepository.InsertAsync(session);
            await _skillRepository.UpdateAsync(skill);

            return await MapToDto(session);
        }

        [Authorize(SkillDevelopmentPermissions.DevelopmentSessions.Manage)]
        public async Task<SkillDevelopmentSessionDto> RecordAssessmentAsync(
            Guid sessionId, 
            RecordAssessmentDto input)
        {
            var session = await _sessionRepository.GetAsync(sessionId);
            CheckOwnership(session);

            var milestone = session.GetCurrentMilestone();
            if (milestone == null)
            {
                throw new BusinessException("SkillDevelopment:NoActiveMilestone");
            }

            var assessment = new AssessmentResult(
                input.Scores,
                input.StrengthAreas,
                input.ImprovementAreas,
                input.OverallNotes);

            milestone.RecordAssessment(assessment);

            await _sessionRepository.UpdateAsync(session);
            return await MapToDto(session);
        }

        [Authorize(SkillDevelopmentPermissions.DevelopmentSessions.Manage)]
        public async Task<SkillDevelopmentSessionDto> AddPracticePlanAsync(
            Guid sessionId, 
            AddPracticePlanDto input)
        {
            var session = await _sessionRepository.GetAsync(sessionId);
            CheckOwnership(session);

            var milestone = session.GetCurrentMilestone();
            if (milestone == null)
            {
                throw new BusinessException("SkillDevelopment:NoActiveMilestone");
            }

            foreach (var itemDto in input.PlanItems)
            {
                var planItem = new PracticePlanItem(
                    itemDto.Activity,
                    itemDto.EstimatedMinutes,
                    itemDto.Objective,
                    itemDto.Resources);

                milestone.AddPracticePlanItem(planItem);
            }

            await _sessionRepository.UpdateAsync(session);
            return await MapToDto(session);
        }

        [Authorize(SkillDevelopmentPermissions.DevelopmentSessions.AddPracticeLog)]
        public async Task<SkillDevelopmentSessionDto> AddPracticeLogAsync(
            Guid sessionId, 
            AddPracticeLogDto input)
        {
            var session = await _sessionRepository.GetAsync(sessionId);
            CheckOwnership(session);

            var milestone = session.GetCurrentMilestone();
            if (milestone == null)
            {
                throw new BusinessException("SkillDevelopment:NoActiveMilestone");
            }

            var log = new PracticeLog(
                milestone.SessionId,
                input.Date,
                input.DurationMinutes,
                input.FocusArea);

            if (input.SelfRating.HasValue)
            {
                log.AddRating(input.SelfRating.Value);
            }

            if (input.ChallengesFaced?.Any() == true)
            {
                foreach (var challenge in input.ChallengesFaced)
                {
                    log.AddChallenge(challenge);
                }
            }

            if (!string.IsNullOrWhiteSpace(input.Notes))
            {
                log.AddNotes(input.Notes);
            }

            milestone.AddPracticeLog(log);

            await _sessionRepository.UpdateAsync(session);
            return await MapToDto(session);
        }

        [Authorize(SkillDevelopmentPermissions.DevelopmentSessions.Manage)]
        public async Task<SkillDevelopmentSessionDto> AddReviewFeedbackAsync(
            Guid sessionId, 
            AddReviewFeedbackDto input)
        {
            var session = await _sessionRepository.GetAsync(sessionId);
            CheckOwnership(session);

            var milestone = session.GetCurrentMilestone();
            if (milestone == null)
            {
                throw new BusinessException("SkillDevelopment:NoActiveMilestone");
            }

            foreach (var feedback in input.Feedback)
            {
                milestone.AddReviewFeedback(feedback.Key, feedback.Value);
            }

            await _sessionRepository.UpdateAsync(session);
            return await MapToDto(session);
        }

        [Authorize(SkillDevelopmentPermissions.DevelopmentSessions.AddApplication)]
        public async Task<SkillDevelopmentSessionDto> AddApplicationExampleAsync(
            Guid sessionId, 
            AddApplicationExampleDto input)
        {
            var session = await _sessionRepository.GetAsync(sessionId);
            CheckOwnership(session);

            var milestone = session.GetCurrentMilestone();
            if (milestone == null)
            {
                throw new BusinessException("SkillDevelopment:NoActiveMilestone");
            }

            var example = new ApplicationExample(
                milestone.SessionId,
                input.Description,
                input.CompletedAt);

            if (input.Outcomes?.Any() == true)
            {
                foreach (var outcome in input.Outcomes)
                {
                    example.AddOutcome(outcome);
                }
            }

            if (input.ProficiencyImpact.HasValue)
            {
                example.SetProficiencyImpact(input.ProficiencyImpact.Value);
            }

            milestone.AddApplication(example);

            await _sessionRepository.UpdateAsync(session);
            return await MapToDto(session);
        }

        [Authorize(SkillDevelopmentPermissions.DevelopmentSessions.Manage)]
        public async Task<SkillDevelopmentSessionDto> RecordMasteryValidationAsync(
            Guid sessionId, 
            RecordMasteryValidationDto input)
        {
            var session = await _sessionRepository.GetAsync(sessionId);
            CheckOwnership(session);

            var milestone = session.GetCurrentMilestone();
            if (milestone == null)
            {
                throw new BusinessException("SkillDevelopment:NoActiveMilestone");
            }

            milestone.RecordMasteryValidation(input.ValidationData);

            await _sessionRepository.UpdateAsync(session);
            return await MapToDto(session);
        }

        [Authorize(SkillDevelopmentPermissions.DevelopmentSessions.Complete)]
        public async Task<SkillDevelopmentSessionDto> CompleteCurrentStepAsync(Guid sessionId)
        {
            var session = await _sessionRepository.GetAsync(sessionId);
            CheckOwnership(session);

            session.CompleteCurrentStep();

            await _sessionRepository.UpdateAsync(session);
            return await MapToDto(session);
        }

        [Authorize(SkillDevelopmentPermissions.DevelopmentSessions.Manage)]
        public async Task<SkillDevelopmentSessionDto> AdvanceToNextStepAsync(Guid sessionId)
        {
            var session = await _sessionRepository.GetAsync(sessionId);
            CheckOwnership(session);

            session.AdvanceToNextStep();

            // If session completed, update skill proficiency
            if (session.Completed)
            {
                var skill = await _skillRepository.GetAsync(session.SkillId);
                var prerequisites = new List<Skill>(); // Load if needed

                var result = _progressionService.ProgressSkill(
                    skill, session, prerequisites, CurrentUser.Id.Value);

                if (result.IsSuccessful)
                {
                    await _skillRepository.UpdateAsync(result.UpdatedSkill);
                }
            }

            await _sessionRepository.UpdateAsync(session);
            return await MapToDto(session);
        }

        [Authorize(SkillDevelopmentPermissions.DevelopmentSessions.Manage)]
        public async Task<SkillDevelopmentSessionDto> PauseAsync(Guid sessionId)
        {
            var session = await _sessionRepository.GetAsync(sessionId);
            CheckOwnership(session);

            session.Pause();

            await _sessionRepository.UpdateAsync(session);
            return await MapToDto(session);
        }

        [Authorize(SkillDevelopmentPermissions.DevelopmentSessions.Manage)]
        public async Task<SkillDevelopmentSessionDto> ResumeAsync(Guid sessionId)
        {
            var session = await _sessionRepository.GetAsync(sessionId);
            CheckOwnership(session);

            session.Resume();

            await _sessionRepository.UpdateAsync(session);
            return await MapToDto(session);
        }

        [Authorize(SkillDevelopmentPermissions.DevelopmentSessions.Manage)]
        public async Task<SkillDevelopmentSessionDto> StartReviewCycleAsync(Guid sessionId)
        {
            var session = await _sessionRepository.GetAsync(sessionId);
            CheckOwnership(session);

            session.StartReviewCycle();

            await _sessionRepository.UpdateAsync(session);
            return await MapToDto(session);
        }

        private void CheckOwnership(SkillDevelopmentSession session)
        {
            if (session.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }
        }

        private async Task<SkillDevelopmentSessionDto> MapToDto(SkillDevelopmentSession session)
        {
            var dto = ObjectMapper.Map<SkillDevelopmentSession, SkillDevelopmentSessionDto>(session);
            
            // Load skill title
            var skill = await _skillRepository.GetAsync(session.SkillId);
            dto.SkillTitle = skill.Title;

            return dto;
        }
    }
}
