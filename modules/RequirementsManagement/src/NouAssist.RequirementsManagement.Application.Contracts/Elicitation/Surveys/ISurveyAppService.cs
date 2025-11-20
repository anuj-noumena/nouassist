using System;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Elicitation.Surveys.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.Elicitation.Surveys
{
    /// <summary>
    /// Application service interface for managing surveys
    /// </summary>
    public interface ISurveyAppService : IApplicationService
    {
        Task<SurveyDto> GetAsync(Guid id);
        Task<SurveyDto> CreateAsync(CreateSurveyDto input);
        Task<SurveyDto> DeployAsync(Guid id, DateTime deploymentDate);
        Task<SurveyResponseDto> SubmitResponseAsync(SubmitSurveyResponseDto input);
        Task<PagedResultDto<SurveyResponseDto>> GetResponsesAsync(Guid surveyId, PagedResultRequestDto input);
        Task<SurveyAnalyticsDto> GetAnalyticsAsync(Guid id);
        Task<bool> CanRespondentAccessAsync(Guid surveyId, Guid respondentId);
    }
}
