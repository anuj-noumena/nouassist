using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.TimeManagement.Calendar.LearningAnalytics.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.TimeManagement.Calendar.LearningAnalytics
{
    public interface ILearningAnalyticsAppService : IApplicationService
    {
        /// <summary>
        /// Get comprehensive learning time report for a user within a date range
        /// </summary>
        Task<LearningTimeReportDto> GetLearningTimeReportAsync(
            Guid userId,
            DateTime fromDate,
            DateTime toDate);

        /// <summary>
        /// Get the most productive time of day for learning based on historical data
        /// </summary>
        Task<string> GetMostProductiveTimeOfDayAsync(Guid userId);

        /// <summary>
        /// Recommend optimal time slots for learning sessions
        /// </summary>
        Task<List<TimeSlotRecommendationDto>> RecommendLearningTimeSlotsAsync(
            Guid userId,
            int durationMinutes,
            DayOfWeek[] preferredDays = null,
            int recommendationCount = 5);
    }
}
