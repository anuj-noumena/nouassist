using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.Elicitation.Aggregates;
using NouAssist.RequirementsManagement.Elicitation.Enums;
using NouAssist.RequirementsManagement.Elicitation.Exceptions;
using NouAssist.RequirementsManagement.Elicitation.Specifications;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.RequirementsManagement.Elicitation.Services
{
    /// <summary>
    /// Domain service for managing elicitation sessions
    /// </summary>
    public class ElicitationSessionManager : DomainService, IElicitationSessionManager
    {
        private readonly IElicitationSessionRepository _sessionRepository;
        private readonly IStakeholderProfileManager _stakeholderManager;
        private readonly ILogger<ElicitationSessionManager> _logger;

        public ElicitationSessionManager(
            IElicitationSessionRepository sessionRepository,
            IStakeholderProfileManager stakeholderManager,
            ILogger<ElicitationSessionManager> logger)
        {
            _sessionRepository = sessionRepository;
            _stakeholderManager = stakeholderManager;
            _logger = logger;
        }

        public async Task<ElicitationSession> CreateAsync(
            string title,
            string description,
            SessionType type,
            SessionSchedule schedule,
            Guid organizerId,
            List<Guid> participantIds = null)
        {
            try
            {
                // Validate organizer
                await _stakeholderManager.ValidateStakeholderAsync(organizerId);

                // Check for scheduling conflicts
                if (!await CanScheduleAsync(schedule))
                {
                    throw new BusinessException("Elicitation:ErrorMessage:SchedulingConflict");
                }

                var session = ElicitationSession.Create(
                    GuidGenerator.Create(),
                    title,
                    description,
                    type,
                    schedule,
                    organizerId,
                    CurrentTenant.Id);

                // Add participants
                if (participantIds != null)
                {
                    foreach (var participantId in participantIds)
                    {
                        await _stakeholderManager.ValidateStakeholderAsync(participantId);
                        session.AddParticipant(participantId);
                    }
                }

                await _sessionRepository.InsertAsync(session);

                _logger.LogInformation("Created elicitation session {Title} of type {Type} scheduled for {StartTime}",
                    title, type, schedule.StartTime);

                return session;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create elicitation session {Title}", title);
                throw;
            }
        }

        public async Task<bool> CanScheduleAsync(SessionSchedule schedule, Guid? excludeSessionId = null)
        {
            var sessions = await _sessionRepository.GetSessionsInTimeRangeAsync(
                schedule.StartTime.AddHours(-1),
                schedule.EndTime.AddHours(1));

            return !sessions.Any(s => s.Id != excludeSessionId &&
                                     s.Status != SessionStatus.Cancelled &&
                                     s.Schedule.IsOverlapping(schedule));
        }

        public async Task<List<ElicitationSession>> GetUpcomingSessionsAsync(int days = 7)
        {
            var endDate = DateTime.UtcNow.AddDays(days);
            var specification = new SessionsInDateRangeSpecification(DateTime.UtcNow, endDate);
            
            return await _sessionRepository.GetListAsync(specification.ToExpression());
        }

        public async Task<List<ElicitationSession>> GetCompletedSessionsWithArtifactsAsync()
        {
            var specification = new SessionsWithArtifactsSpecification();
            return await _sessionRepository.GetListAsync(specification.ToExpression());
        }

        public async Task ValidateSessionAsync(Guid sessionId)
        {
            var session = await _sessionRepository.FindAsync(sessionId);
            if (session == null)
            {
                throw new SessionNotFoundException(sessionId);
            }
        }

        public async Task<SessionStatistics> GetSessionStatisticsAsync(Guid sessionId)
        {
            var session = await _sessionRepository.GetAsync(sessionId);
            
            return new SessionStatistics
            {
                SessionId = sessionId,
                ParticipantCount = session.Participants.Count,
                AttendedCount = session.Participants.Count(p => p.HasAttended),
                ArtifactsGenerated = session.GeneratedArtifacts.Count,
                Duration = session.Schedule.Duration,
                CompletionDate = session.Status == SessionStatus.Completed ? session.LastModificationTime : null,
                SurveyResponseCount = session.SurveyDefinition?.GetResponseCount() ?? 0,
                SurveyCompletionRate = session.SurveyDefinition?.GetCompletionRate() ?? 0.0
            };
        }
    }
}
