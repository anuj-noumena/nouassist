using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Elicitation.Aggregates;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.RequirementsManagement.Elicitation.Services
{
    /// <summary>
    /// Domain service interface for managing elicitation sessions
    /// </summary>
    public interface IElicitationSessionManager : IDomainService
    {
        Task<ElicitationSession> CreateAsync(
            string title,
            string description,
            SessionType type,
            SessionSchedule schedule,
            Guid organizerId,
            List<Guid> participantIds = null);

        Task<bool> CanScheduleAsync(SessionSchedule schedule, Guid? excludeSessionId = null);

        Task<List<ElicitationSession>> GetUpcomingSessionsAsync(int days = 7);

        Task<List<ElicitationSession>> GetCompletedSessionsWithArtifactsAsync();

        Task ValidateSessionAsync(Guid sessionId);

        Task<SessionStatistics> GetSessionStatisticsAsync(Guid sessionId);
    }
}
