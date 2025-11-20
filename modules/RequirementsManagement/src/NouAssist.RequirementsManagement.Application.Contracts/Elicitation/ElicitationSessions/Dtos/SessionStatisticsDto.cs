using System;

namespace NouAssist.RequirementsManagement.Elicitation.ElicitationSessions.Dtos
{
    /// <summary>
    /// DTO for session statistics
    /// </summary>
    public class SessionStatisticsDto
    {
        public Guid SessionId { get; set; }
        public int ParticipantCount { get; set; }
        public int AttendedCount { get; set; }
        public int ArtifactsGenerated { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int SurveyResponseCount { get; set; }
        public double SurveyCompletionRate { get; set; }
        public double AttendanceRate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
