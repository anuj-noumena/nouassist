using System;

namespace NouAssist.RequirementsManagement.Elicitation.Interviews.Dtos
{
    /// <summary>
    /// DTO for interview details
    /// </summary>
    public class InterviewDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Location { get; set; }
        public Guid OrganizerId { get; set; }
        public string? OrganizerName { get; set; }
        public Guid IntervieweeId { get; set; }
        public string? IntervieweeName { get; set; }
        public InterviewRecordDetailDto? Record { get; set; }
    }

    /// <summary>
    /// DTO for interview record details
    /// </summary>
    public class InterviewRecordDetailDto
    {
        public Guid Id { get; set; }
        public DateTime RecordedAt { get; set; }
        public string? TranscriptText { get; set; }
        public string? RecordingPath { get; set; }
        public string? FileName { get; set; }
        public long FileSizeBytes { get; set; }
        public string? MimeType { get; set; }
        public bool HasTranscript { get; set; }
        public bool HasRecording { get; set; }
    }
}
