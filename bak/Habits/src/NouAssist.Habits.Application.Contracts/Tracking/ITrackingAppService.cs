using System;
using System.Threading.Tasks;
using NouAssist.Habits.Tracking.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Habits.Tracking
{
    public interface ITrackingAppService : IApplicationService
    {
        // Health Tracking
        Task<HealthTrackingDto> GetHealthTrackingAsync(DateTime date);
        Task<HealthTrackingDto> UpdateHealthTrackingAsync(UpdateHealthTrackingDto input);
        Task<HealthTrackingDto> LogHealthActionAsync(LogHealthActionDto input);
        
        // Energy Tracking
        Task<EnergyTrackingDto> GetEnergyTrackingAsync(DateTime date);
        Task<EnergyTrackingDto> UpdateEnergyTrackingAsync(UpdateEnergyTrackingDto input);
        
        // Mood Tracking
        Task<MoodTrackingDto> GetMoodTrackingAsync(DateTime date);
        Task<MoodTrackingDto> UpdateMoodTrackingAsync(UpdateMoodTrackingDto input);
        
        // Focus Session
        Task<FocusSessionDto> GetFocusSessionAsync(DateTime date);
        Task<FocusSessionDto> CompletePomodoroAsync(CompletePomodoroDto input);
        Task<FocusSessionDto> UpdateFocusSessionAsync(UpdateFocusSessionDto input);
        
        // Wellness Report
        Task<WellnessReportDto> GetWellnessReportAsync(GetWellnessReportDto input);
    }
}
