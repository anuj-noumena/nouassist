using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.Elicitation.Aggregates;
using NouAssist.RequirementsManagement.Elicitation.Entities;
using NouAssist.RequirementsManagement.Elicitation.Localization;
using NouAssist.RequirementsManagement.Elicitation.Permissions;
using NouAssist.RequirementsManagement.Elicitation.Services;
using NouAssist.RequirementsManagement.Elicitation.Surveys.Dtos;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.Elicitation.Surveys
{
    [Authorize(ElicitationPermissions.Surveys.Default)]
    public class SurveyAppService : ApplicationService, ISurveyAppService
    {
        private readonly IRepository<ElicitationSession, Guid> _sessionRepository;
        private readonly ISurveyDeploymentService _surveyDeploymentService;
        private readonly ICurrentUser _currentUser;
        private readonly IClock _clock;
        private readonly ILogger<SurveyAppService> _logger;

        public SurveyAppService(
            IRepository<ElicitationSession, Guid> sessionRepository,
            ISurveyDeploymentService surveyDeploymentService,
            ICurrentUser currentUser,
            IClock clock,
            ILogger<SurveyAppService> logger)
        {
            _sessionRepository = sessionRepository;
            _surveyDeploymentService = surveyDeploymentService;
            _currentUser = currentUser;
            _clock = clock;
            _logger = logger;
            LocalizationResource = typeof(ElicitationResource);
        }

        [Authorize(ElicitationPermissions.Surveys.ViewResults)]
        public virtual async Task<SurveyDto> GetAsync(Guid id)
        {
            try
            {
                var session = await _sessionRepository.GetAsync(id, includeDetails: true);

                if (session.Type != SessionType.Survey || session.SurveyDefinition == null)
                {
                    throw new UserFriendlyException(L["Elicitation:ErrorMessage:InvalidArtifactType"]);
                }

                return ObjectMapper.Map<ElicitationSession, SurveyDto>(session);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Survey not found: {SurveyId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving survey: {SurveyId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.Surveys.Create)]
        public virtual async Task<SurveyDto> CreateAsync(CreateSurveyDto input)
        {
            try
            {
                var schedule = new SessionSchedule(input.StartTime, input.EndTime);

                var questions = input.Questionnaire.Pages
                    .SelectMany(p => p.Questions.Select(q => new SurveyQuestion(q.Id, q.Text, q.Type, q.IsRequired, q.Options)))
                    .ToList();

                var pages = input.Questionnaire.Pages
                    .Select(p => new SurveyPage(p.Title, p.Questions.Select(q => 
                        new SurveyQuestion(q.Id, q.Text, q.Type, q.IsRequired, q.Options)).ToList()))
                    .ToList();

                var questionnaire = new QuestionnaireStructure(pages, input.Questionnaire.Logic);

                var session = await _surveyDeploymentService.CreateSurveySessionAsync(
                    input.Title,
                    input.Description,
                    schedule,
                    input.OrganizerId,
                    questionnaire,
                    input.TargetStakeholderIds
                );

                _logger.LogInformation("Survey created: {Title} ({Id})", session.Title, session.Id);
                return ObjectMapper.Map<ElicitationSession, SurveyDto>(session);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error creating survey: {Title}", input.Title);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating survey: {Title}", input.Title);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.Surveys.Deploy)]
        public virtual async Task<SurveyDto> DeployAsync(Guid id, DateTime deploymentDate)
        {
            try
            {
                await _surveyDeploymentService.DeploySurveyAsync(id, deploymentDate);
                var session = await _sessionRepository.GetAsync(id, includeDetails: true);

                _logger.LogInformation("Survey deployed: {Title} ({Id})", session.Title, session.Id);
                return ObjectMapper.Map<ElicitationSession, SurveyDto>(session);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error deploying survey: {SurveyId}", id);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Survey not found: {SurveyId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deploying survey: {SurveyId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.Surveys.Respond)]
        public virtual async Task<SurveyResponseDto> SubmitResponseAsync(SubmitSurveyResponseDto input)
        {
            try
            {
                var responseData = new ResponseData(input.Answers, input.StartedAt, input.SubmittedAt);
                var response = await _surveyDeploymentService.SubmitResponseAsync(input.SurveyId, input.RespondentId, responseData);

                _logger.LogInformation("Survey response submitted: {SurveyId} by {RespondentId}", input.SurveyId, input.RespondentId);
                return ObjectMapper.Map<SurveyResponse, SurveyResponseDto>(response);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error submitting survey response: {SurveyId}", input.SurveyId);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting survey response: {SurveyId}", input.SurveyId);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.Surveys.ViewResults)]
        public virtual async Task<PagedResultDto<SurveyResponseDto>> GetResponsesAsync(Guid surveyId, PagedResultRequestDto input)
        {
            try
            {
                var session = await _sessionRepository.GetAsync(surveyId, includeDetails: true);

                if (session.Type != SessionType.Survey || session.SurveyDefinition == null)
                {
                    throw new UserFriendlyException(L["Elicitation:ErrorMessage:InvalidArtifactType"]);
                }

                var responses = session.SurveyDefinition.Responses
                    .OrderByDescending(r => r.CreatedAt)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToList();

                var totalCount = session.SurveyDefinition.Responses.Count;

                return new PagedResultDto<SurveyResponseDto>(
                    totalCount,
                    ObjectMapper.Map<List<SurveyResponse>, List<SurveyResponseDto>>(responses)
                );
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Survey not found: {SurveyId}", surveyId);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", surveyId]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving survey responses: {SurveyId}", surveyId);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.Surveys.ViewResults)]
        public virtual async Task<SurveyAnalyticsDto> GetAnalyticsAsync(Guid id)
        {
            try
            {
                var analytics = await _surveyDeploymentService.GetSurveyAnalyticsAsync(id);

                _logger.LogInformation("Survey analytics retrieved: {SurveyId}", id);
                return ObjectMapper.Map<SurveyAnalytics, SurveyAnalyticsDto>(analytics);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error retrieving survey analytics: {SurveyId}", id);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Survey not found: {SurveyId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving survey analytics: {SurveyId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        public virtual async Task<bool> CanRespondentAccessAsync(Guid surveyId, Guid respondentId)
        {
            try
            {
                return await _surveyDeploymentService.CanRespondentAccessSurveyAsync(surveyId, respondentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking survey access: {SurveyId} for {RespondentId}", surveyId, respondentId);
                return false;
            }
        }
    }
}
