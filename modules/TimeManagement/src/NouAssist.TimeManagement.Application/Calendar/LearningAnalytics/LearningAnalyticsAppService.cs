using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.TimeManagement.Calendar.LearningAnalytics.Dtos;
using NouAssist.TimeManagement.Calendar.Services;
using Volo.Abp.Application.Services;

namespace NouAssist.TimeManagement.Calendar.LearningAnalytics
{
    [Authorize]
    public class LearningAnalyticsAppService : ApplicationService, ILearningAnalyticsAppService
    {
        private readonly LearningTimeAnalyticsService _analyticsService;
        private readonly LearningScheduleOptimizerService _schedulerService;

        public LearningAnalyticsAppService(
            LearningTimeAnalyticsService analyticsService,
            LearningScheduleOptimizerService schedulerService)
        {
            _analyticsService = analyticsService;
            _schedulerService = schedulerService;
        }

        public async Task<LearningTimeReportDto> GetLearningTimeReportAsync(
            Guid userId,
            DateTime fromDate,
            DateTime toDate)
        {
            var report = await _analyticsService.GetLearningTimeReportAsync(userId, fromDate, toDate);

            return new LearningTimeReportDto
            {
                UserId = report.UserId,
                FromDate = report.FromDate,
                ToDate = report.ToDate,
                TotalMinutes = report.TotalMinutes,
                ByPhase = report.ByPhase,
                AverageSessionLengthMinutes = report.AverageSessionLengthMinutes,
                MostProductiveTimeOfDay = report.MostProductiveTimeOfDay,
                SessionCount = report.SessionCount,
                LongestSessionMinutes = report.LongestSessionMinutes,
                ShortestSessionMinutes = report.ShortestSessionMinutes
            };
        }

        public async Task<string> GetMostProductiveTimeOfDayAsync(Guid userId)
        {
            var thirtyDaysAgo = DateTime.Today.AddDays(-30);
            var report = await _analyticsService.GetLearningTimeReportAsync(
                userId, thirtyDaysAgo, DateTime.Today);

            return report.MostProductiveTimeOfDay;
        }

        public async Task<List<TimeSlotRecommendationDto>> RecommendLearningTimeSlotsAsync(
            Guid userId,
            int durationMinutes,
            DayOfWeek[] preferredDays = null,
            int recommendationCount = 5)
        {
            var recommendations = await _schedulerService.RecommendLearningTimesAsync(
                userId, durationMinutes, preferredDays, recommendationCount);

            return recommendations.Select(r => new TimeSlotRecommendationDto
            {
                Date = r.Date,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                Score = r.Score,
                Reason = r.Reason
            }).ToList();
        }
    }
}
