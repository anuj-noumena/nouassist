using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.Elicitation.Aggregates;
using NouAssist.RequirementsManagement.Elicitation.Enums;
using NouAssist.RequirementsManagement.Elicitation.Interviews.Dtos;
using NouAssist.RequirementsManagement.Elicitation.Localization;
using NouAssist.RequirementsManagement.Elicitation.Permissions;
using NouAssist.RequirementsManagement.Elicitation.Services;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.Elicitation.Interviews
{
    [Authorize(ElicitationPermissions.Interviews.Default)]
    public class InterviewAppService : ApplicationService, IInterviewAppService
    {
        private readonly IRepository<ElicitationSession, Guid> _sessionRepository;
        private readonly IInterviewScheduler _interviewScheduler;
        private readonly ICurrentUser _currentUser;
        private readonly IClock _clock;
        private readonly ILogger<InterviewAppService> _logger;

        public InterviewAppService(
            IRepository<ElicitationSession, Guid> sessionRepository,
            IInterviewScheduler interviewScheduler,
            ICurrentUser currentUser,
            IClock clock,
            ILogger<InterviewAppService> logger)
        {
            _sessionRepository = sessionRepository;
            _interviewScheduler = interviewScheduler;
            _currentUser = currentUser;
            _clock = clock;
            _logger = logger;
            LocalizationResource = typeof(ElicitationResource);
        }

        [Authorize(ElicitationPermissions.Interviews.ViewRecords)]
        public virtual async Task<InterviewDto> GetAsync(Guid id)
        {
            try
            {
                var session = await _sessionRepository.GetAsync(id, includeDetails: true);

                if (session.Type != SessionType.Interview)
                {
                    throw new UserFriendlyException(L["Elicitation:ErrorMessage:InvalidArtifactType"]);
                }

                return ObjectMapper.Map<ElicitationSession, InterviewDto>(session);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Interview not found: {InterviewId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving interview: {InterviewId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.Interviews.Schedule)]
        public virtual async Task<InterviewDto> ScheduleAsync(ScheduleInterviewDto input)
        {
            try
            {
                var schedule = new SessionSchedule(input.StartTime, input.EndTime, input.Location);

                var session = await _interviewScheduler.ScheduleInterviewAsync(
                    input.Title,
                    input.Description,
                    schedule,
                    input.OrganizerId,
                    input.IntervieweeId
                );

                _logger.LogInformation("Interview scheduled: {Title} ({Id})", session.Title, session.Id);
                return ObjectMapper.Map<ElicitationSession, InterviewDto>(session);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error scheduling interview: {Title}", input.Title);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error scheduling interview: {Title}", input.Title);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.Interviews.Schedule)]
        public virtual async Task<InterviewDto> RescheduleAsync(Guid id, RescheduleInterviewDto input)
        {
            try
            {
                var newSchedule = new SessionSchedule(input.StartTime, input.EndTime, input.Location);
                await _interviewScheduler.RescheduleInterviewAsync(id, newSchedule);

                var session = await _sessionRepository.GetAsync(id, includeDetails: true);

                _logger.LogInformation("Interview rescheduled: {Title} ({Id})", session.Title, session.Id);
                return ObjectMapper.Map<ElicitationSession, InterviewDto>(session);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error rescheduling interview: {InterviewId}", id);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Interview not found: {InterviewId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rescheduling interview: {InterviewId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.Interviews.Schedule)]
        public virtual async Task<InterviewDto> CancelAsync(Guid id, string reason)
        {
            try
            {
                await _interviewScheduler.CancelInterviewAsync(id, reason);
                var session = await _sessionRepository.GetAsync(id, includeDetails: true);

                _logger.LogInformation("Interview cancelled: {Title} ({Id}) - {Reason}", session.Title, session.Id, reason);
                return ObjectMapper.Map<ElicitationSession, InterviewDto>(session);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error cancelling interview: {InterviewId}", id);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Interview not found: {InterviewId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling interview: {InterviewId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.Interviews.Conduct)]
        public virtual async Task<InterviewDto> UpdateRecordAsync(Guid id, UpdateInterviewRecordDto input)
        {
            try
            {
                var session = await _sessionRepository.GetAsync(id, includeDetails: true);

                if (session.Type != SessionType.Interview)
                {
                    throw new UserFriendlyException(L["Elicitation:ErrorMessage:InvalidArtifactType"]);
                }

                var transcriptRecording = new TranscriptRecordingRef(
                    input.TranscriptText,
                    input.RecordingPath,
                    input.FileName,
                    input.FileSizeBytes,
                    input.MimeType
                );

                session.SetInterviewRecord(transcriptRecording);
                await _sessionRepository.UpdateAsync(session);

                _logger.LogInformation("Interview record updated: {Title} ({Id})", session.Title, session.Id);
                return ObjectMapper.Map<ElicitationSession, InterviewDto>(session);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error updating interview record: {InterviewId}", id);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Interview not found: {InterviewId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating interview record: {InterviewId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.Interviews.Schedule)]
        public virtual async Task<List<InterviewDto>> FindAvailableTimeSlotsAsync(List<Guid> participantIds, TimeSpan duration, DateTime startDate, DateTime endDate)
        {
            try
            {
                var availableSlots = await _interviewScheduler.FindAvailableTimeSlotsAsync(participantIds, duration, startDate, endDate);

                _logger.LogInformation("Found {Count} available time slots for participants", availableSlots.Count);
                return ObjectMapper.Map<List<ElicitationSession>, List<InterviewDto>>(availableSlots);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding available time slots");
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }
    }
}
