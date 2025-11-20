using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Elicitation.Interviews.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.Elicitation.Interviews
{
    /// <summary>
    /// Application service interface for managing interviews
    /// </summary>
    public interface IInterviewAppService : IApplicationService
    {
        Task<InterviewDto> GetAsync(Guid id);
        Task<InterviewDto> ScheduleAsync(ScheduleInterviewDto input);
        Task<InterviewDto> RescheduleAsync(Guid id, RescheduleInterviewDto input);
        Task<InterviewDto> CancelAsync(Guid id, string reason);
        Task<InterviewDto> UpdateRecordAsync(Guid id, UpdateInterviewRecordDto input);
        Task<List<InterviewDto>> FindAvailableTimeSlotsAsync(List<Guid> participantIds, TimeSpan duration, DateTime startDate, DateTime endDate);
    }
}
