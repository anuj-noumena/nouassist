using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.Elicitation.Aggregates;
using NouAssist.RequirementsManagement.Elicitation.ElicitationSessions.Dtos;
using NouAssist.RequirementsManagement.Elicitation.Enums;
using NouAssist.RequirementsManagement.Elicitation.Exceptions;
using NouAssist.RequirementsManagement.Elicitation.Localization;
using NouAssist.RequirementsManagement.Elicitation.Permissions;
using NouAssist.RequirementsManagement.Elicitation.Services;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.Elicitation.ElicitationSessions
{
    [Authorize(ElicitationPermissions.ElicitationSessions.Default)]
    public class ElicitationSessionAppService : ApplicationService, IElicitationSessionAppService
    {
        private readonly IRepository<ElicitationSession, Guid> _sessionRepository;
        private readonly IElicitationSessionManager _sessionManager;
        private readonly ICurrentUser _currentUser;
        private readonly IClock _clock;
        private readonly ILogger<ElicitationSessionAppService> _logger;

        public ElicitationSessionAppService(
            IRepository<ElicitationSession, Guid> sessionRepository,
            IElicitationSessionManager sessionManager,
            ICurrentUser currentUser,
            IClock clock,
            ILogger<ElicitationSessionAppService> logger)
        {
            _sessionRepository = sessionRepository;
            _sessionManager = sessionManager;
            _currentUser = currentUser;
            _clock = clock;
            _logger = logger;
            LocalizationResource = typeof(ElicitationResource);
        }

        [Authorize(ElicitationPermissions.ElicitationSessions.View)]
        public virtual async Task<ElicitationSessionDto> GetAsync(Guid id)
        {
            try
            {
                var session = await _sessionRepository.GetAsync(id, includeDetails: true);
                return ObjectMapper.Map<ElicitationSession, ElicitationSessionDto>(session);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Session not found: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving session: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationSessions.View)]
        public virtual async Task<PagedResultDto<ElicitationSessionDto>> GetListAsync(GetElicitationSessionListDto input)
        {
            try
            {
                var queryable = await _sessionRepository.GetQueryableAsync();

                // Apply filters
                queryable = queryable.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    x => x.Title.Contains(input.Filter!) || x.Description.Contains(input.Filter!));

                queryable = queryable.WhereIf(input.Type.HasValue, x => x.Type == input.Type.Value);
                queryable = queryable.WhereIf(input.Status.HasValue, x => x.Status == input.Status.Value);
                queryable = queryable.WhereIf(input.OrganizerId.HasValue, x => x.OrganizerId == input.OrganizerId.Value);

                if (input.StartDate.HasValue)
                    queryable = queryable.Where(x => x.Schedule.StartTime >= input.StartDate.Value);
                if (input.EndDate.HasValue)
                    queryable = queryable.Where(x => x.Schedule.EndTime <= input.EndDate.Value);

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var sessions = await AsyncExecuter.ToListAsync(
                    queryable.OrderBy(input.Sorting ?? nameof(ElicitationSession.Schedule.StartTime))
                             .PageBy(input)
                );

                return new PagedResultDto<ElicitationSessionDto>(
                    totalCount,
                    ObjectMapper.Map<List<ElicitationSession>, List<ElicitationSessionDto>>(sessions)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving session list");
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationSessions.Create)]
        public virtual async Task<ElicitationSessionDto> CreateAsync(CreateElicitationSessionDto input)
        {
            try
            {
                var schedule = new SessionSchedule(
                    input.Schedule.StartTime,
                    input.Schedule.EndTime,
                    input.Schedule.Location,
                    input.Schedule.TimeZone
                );

                var session = await _sessionManager.CreateAsync(
                    input.Title,
                    input.Description,
                    input.Type,
                    schedule,
                    input.OrganizerId,
                    input.ParticipantIds
                );

                _logger.LogInformation("Session created: {Title} ({Id})", session.Title, session.Id);
                return ObjectMapper.Map<ElicitationSession, ElicitationSessionDto>(session);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error creating session: {Title}", input.Title);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating session: {Title}", input.Title);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationSessions.Update)]
        public virtual async Task<ElicitationSessionDto> UpdateAsync(Guid id, UpdateElicitationSessionDto input)
        {
            try
            {
                var session = await _sessionRepository.GetAsync(id, includeDetails: true);

                // Check if session can be updated
                if (session.Status == SessionStatus.Completed || session.Status == SessionStatus.Cancelled)
                {
                    throw new UserFriendlyException(L["Elicitation:ErrorMessage:InvalidStatusTransition"]);
                }

                // Update basic properties using reflection (since setters are private)
                var titleProperty = typeof(ElicitationSession).GetProperty("Title", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?? typeof(ElicitationSession).GetProperty("Title");
                titleProperty?.SetValue(session, input.Title);

                var descProperty = typeof(ElicitationSession).GetProperty("Description", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?? typeof(ElicitationSession).GetProperty("Description");
                descProperty?.SetValue(session, input.Description);

                // Update schedule
                var schedule = new SessionSchedule(
                    input.Schedule.StartTime,
                    input.Schedule.EndTime,
                    input.Schedule.Location,
                    input.Schedule.TimeZone
                );
                var scheduleProperty = typeof(ElicitationSession).GetProperty("Schedule", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?? typeof(ElicitationSession).GetProperty("Schedule");
                scheduleProperty?.SetValue(session, schedule);

                // Update participants
                var currentParticipantIds = session.Participants.Select(p => p.StakeholderId).ToList();
                var participantsToRemove = currentParticipantIds.Except(input.ParticipantIds).ToList();
                var participantsToAdd = input.ParticipantIds.Except(currentParticipantIds).ToList();

                foreach (var participantId in participantsToRemove)
                {
                    session.RemoveParticipant(participantId);
                }

                foreach (var participantId in participantsToAdd)
                {
                    session.AddParticipant(participantId);
                }

                await _sessionRepository.UpdateAsync(session);

                _logger.LogInformation("Session updated: {Title} ({Id})", session.Title, session.Id);
                return ObjectMapper.Map<ElicitationSession, ElicitationSessionDto>(session);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Session not found for update: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating session: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationSessions.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var session = await _sessionRepository.GetAsync(id);

                // Check if session can be deleted
                if (session.Status == SessionStatus.InProgress)
                {
                    throw new UserFriendlyException(L["Elicitation:ErrorMessage:CannotDeleteActiveSession"]);
                }

                await _sessionRepository.DeleteAsync(id);
                _logger.LogInformation("Session deleted: {SessionId}", id);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Session not found for deletion: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting session: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationSessions.Schedule)]
        public virtual async Task<ElicitationSessionDto> ScheduleAsync(Guid id)
        {
            try
            {
                var session = await _sessionRepository.GetAsync(id, includeDetails: true);
                session.Schedule();
                await _sessionRepository.UpdateAsync(session);

                _logger.LogInformation("Session scheduled: {Title} ({Id})", session.Title, session.Id);
                return ObjectMapper.Map<ElicitationSession, ElicitationSessionDto>(session);
            }
            catch (InvalidSessionStatusTransitionException ex)
            {
                _logger.LogWarning(ex, "Invalid status transition for session: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:InvalidSessionStatusTransition"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Session not found: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error scheduling session: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationSessions.Manage)]
        public virtual async Task<ElicitationSessionDto> StartAsync(Guid id)
        {
            try
            {
                var session = await _sessionRepository.GetAsync(id, includeDetails: true);
                session.Start();
                await _sessionRepository.UpdateAsync(session);

                _logger.LogInformation("Session started: {Title} ({Id})", session.Title, session.Id);
                return ObjectMapper.Map<ElicitationSession, ElicitationSessionDto>(session);
            }
            catch (InvalidSessionStatusTransitionException ex)
            {
                _logger.LogWarning(ex, "Invalid status transition for session: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:InvalidSessionStatusTransition"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Session not found: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting session: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationSessions.Manage)]
        public virtual async Task<ElicitationSessionDto> CompleteAsync(Guid id)
        {
            try
            {
                var session = await _sessionRepository.GetAsync(id, includeDetails: true);
                session.Complete();
                await _sessionRepository.UpdateAsync(session);

                _logger.LogInformation("Session completed: {Title} ({Id})", session.Title, session.Id);
                return ObjectMapper.Map<ElicitationSession, ElicitationSessionDto>(session);
            }
            catch (InvalidSessionStatusTransitionException ex)
            {
                _logger.LogWarning(ex, "Invalid status transition for session: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:InvalidSessionStatusTransition"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Session not found: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing session: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationSessions.Cancel)]
        public virtual async Task<ElicitationSessionDto> CancelAsync(Guid id)
        {
            try
            {
                var session = await _sessionRepository.GetAsync(id, includeDetails: true);
                session.Cancel();
                await _sessionRepository.UpdateAsync(session);

                _logger.LogInformation("Session cancelled: {Title} ({Id})", session.Title, session.Id);
                return ObjectMapper.Map<ElicitationSession, ElicitationSessionDto>(session);
            }
            catch (InvalidSessionStatusTransitionException ex)
            {
                _logger.LogWarning(ex, "Invalid status transition for session: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:InvalidSessionStatusTransition"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Session not found: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling session: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationSessions.Schedule)]
        public virtual async Task<ElicitationSessionDto> PostponeAsync(Guid id)
        {
            try
            {
                var session = await _sessionRepository.GetAsync(id, includeDetails: true);
                session.Postpone();
                await _sessionRepository.UpdateAsync(session);

                _logger.LogInformation("Session postponed: {Title} ({Id})", session.Title, session.Id);
                return ObjectMapper.Map<ElicitationSession, ElicitationSessionDto>(session);
            }
            catch (InvalidSessionStatusTransitionException ex)
            {
                _logger.LogWarning(ex, "Invalid status transition for session: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:InvalidSessionStatusTransition"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Session not found: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error postponing session: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationSessions.Update)]
        public virtual async Task AddParticipantAsync(Guid sessionId, Guid stakeholderId, bool isRequired = false)
        {
            try
            {
                var session = await _sessionRepository.GetAsync(sessionId, includeDetails: true);
                session.AddParticipant(stakeholderId, isRequired);
                await _sessionRepository.UpdateAsync(session);

                _logger.LogInformation("Participant added to session: {StakeholderId} -> {SessionId}", stakeholderId, sessionId);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Session not found: {SessionId}", sessionId);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", sessionId]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding participant to session: {SessionId}", sessionId);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationSessions.Update)]
        public virtual async Task RemoveParticipantAsync(Guid sessionId, Guid stakeholderId)
        {
            try
            {
                var session = await _sessionRepository.GetAsync(sessionId, includeDetails: true);
                session.RemoveParticipant(stakeholderId);
                await _sessionRepository.UpdateAsync(session);

                _logger.LogInformation("Participant removed from session: {StakeholderId} -> {SessionId}", stakeholderId, sessionId);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Session not found: {SessionId}", sessionId);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", sessionId]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing participant from session: {SessionId}", sessionId);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationSessions.Manage)]
        public virtual async Task MarkParticipantAsAttendedAsync(Guid sessionId, Guid stakeholderId)
        {
            try
            {
                var session = await _sessionRepository.GetAsync(sessionId, includeDetails: true);
                var participant = session.Participants.FirstOrDefault(p => p.StakeholderId == stakeholderId);
                
                if (participant == null)
                {
                    throw new UserFriendlyException(L["Elicitation:ErrorMessage:ParticipantNotFound"]);
                }

                participant.MarkAsAttended(_clock.Now);
                await _sessionRepository.UpdateAsync(session);

                _logger.LogInformation("Participant marked as attended: {StakeholderId} -> {SessionId}", stakeholderId, sessionId);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Session not found: {SessionId}", sessionId);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", sessionId]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking participant as attended: {SessionId}", sessionId);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationSessions.View)]
        public virtual async Task<SessionStatisticsDto> GetStatisticsAsync(Guid id)
        {
            try
            {
                var statistics = await _sessionManager.GetSessionStatisticsAsync(id);
                return ObjectMapper.Map<SessionStatistics, SessionStatisticsDto>(statistics);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Session not found: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:SessionNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving session statistics: {SessionId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationSessions.View)]
        public virtual async Task<ListResultDto<ElicitationSessionDto>> GetUpcomingSessionsAsync(int days = 7)
        {
            try
            {
                var upcomingSessions = await _sessionManager.GetUpcomingSessionsAsync(days);
                return new ListResultDto<ElicitationSessionDto>(
                    ObjectMapper.Map<List<ElicitationSession>, List<ElicitationSessionDto>>(upcomingSessions)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving upcoming sessions");
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }
    }
}
