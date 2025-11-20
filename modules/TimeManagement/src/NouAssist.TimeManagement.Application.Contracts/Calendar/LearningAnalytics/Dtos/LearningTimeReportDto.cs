using System;
using System.Collections.Generic;

namespace NouAssist.TimeManagement.Calendar.LearningAnalytics.Dtos
{
    public class LearningTimeReportDto
    {
        public Guid UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalMinutes { get; set; }
        public Dictionary<string, int> ByPhase { get; set; } = new();
        public int AverageSessionLengthMinutes { get; set; }
        public string MostProductiveTimeOfDay { get; set; }
        public int SessionCount { get; set; }
        public int LongestSessionMinutes { get; set; }
        public int ShortestSessionMinutes { get; set; }
    }
}
