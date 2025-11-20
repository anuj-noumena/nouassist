using System;

namespace NouAssist.TimeManagement.Calendar.LearningAnalytics.Dtos
{
    public class TimeSlotRecommendationDto
    {
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Score { get; set; }
        public string Reason { get; set; }
    }
}
