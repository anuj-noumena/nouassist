using System;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Elicitation.ElicitationSessions.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.Elicitation.ElicitationSessions
{
    /// <summary>
    /// Application service interface for managing elicitation sessions
    /// </summary>
    public interface IElicitationSessionAppService : IApplicationService
    {
        Task<ElicitationSessionDto> GetAsync(Guid id);
        Task<PagedResultDto<ElicitationSessionDto>> GetListAsync(GetElicitationSessionListDto input);
        Task<ElicitationSessionDto> CreateAsync(CreateElicitationSessionDto input);
        Task<ElicitationSessionDto> UpdateAsync(Guid id, UpdateElicitationSessionDto input);
        Task DeleteAsync(Guid id);
        Task<ElicitationSessionDto> ScheduleAsync(Guid id);
        Task<ElicitationSessionDto> StartAsync(Guid id);
        Task<ElicitationSessionDto> CompleteAsync(Guid id);
        Task<ElicitationSessionDto> CancelAsync(Guid id);
        Task<ElicitationSessionDto> PostponeAsync(Guid id);
        Task AddParticipantAsync(Guid sessionId, Guid stakeholderId, bool isRequired = false);
        Task RemoveParticipantAsync(Guid sessionId, Guid stakeholderId);
        Task MarkParticipantAsAttendedAsync(Guid sessionId, Guid stakeholderId);
        Task<SessionStatisticsDto> GetStatisticsAsync(Guid id);
        Task<ListResultDto<ElicitationSessionDto>> GetUpcomingSessionsAsync(int days = 7);
    }
}
