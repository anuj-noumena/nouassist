using System;
using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.LearningResources.Dtos
{
    public class LearningStatsDto
    {
        public int TotalResourcesStarted { get; set; }
        public int ResourcesCompleted { get; set; }
        public int ResourcesInProgress { get; set; }
        public int ResourcesAbandoned { get; set; }
        public TimeSpan EstimatedLearningTime { get; set; }
        public string EstimatedLearningTimeFormatted => FormatTimeSpan(EstimatedLearningTime);
        public List<string> TopLearningCategories { get; set; }

        public LearningStatsDto()
        {
            TopLearningCategories = new List<string>();
        }

        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan.TotalHours < 1)
            {
                return $"{timeSpan.Minutes} minutes";
            }
            else if (timeSpan.TotalDays < 1)
            {
                return $"{Math.Floor(timeSpan.TotalHours)} hours {timeSpan.Minutes} minutes";
            }
            else
            {
                return $"{Math.Floor(timeSpan.TotalDays)} days {timeSpan.Hours} hours";
            }
        }
    }
}
