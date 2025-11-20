using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.Elicitation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Elicitation.ElicitationSessions.Dtos
{
    /// <summary>
    /// DTO representing an elicitation session
    /// </summary>
    public class ElicitationSessionDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public SessionType Type { get; set; }
        public SessionStatus Status { get; set; }
        public SessionScheduleDto Schedule { get; set; } = new();
        public Guid OrganizerId { get; set; }
        public string? OrganizerName { get; set; }
        public List<ElicitationSessionParticipantDto> Participants { get; set; } = new();
        public int ArtifactsGenerated { get; set; }
        public SurveyDefinitionDto? SurveyDefinition { get; set; }
        public InterviewRecordDto? InterviewRecord { get; set; }
    }

    /// <summary>
    /// DTO for session schedule information
    /// </summary>
    public class SessionScheduleDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Location { get; set; }
        public string TimeZone { get; set; } = "UTC";
        public TimeSpan Duration { get; set; }
    }

    /// <summary>
    /// DTO for session participants
    /// </summary>
    public class ElicitationSessionParticipantDto
    {
        public Guid StakeholderId { get; set; }
        public string? StakeholderName { get; set; }
        public bool IsRequired { get; set; }
        public bool HasAttended { get; set; }
        public DateTime? AttendedAt { get; set; }
    }

    /// <summary>
    /// DTO for survey definition summary
    /// </summary>
    public class SurveyDefinitionDto
    {
        public Guid Id { get; set; }
        public bool IsDeployed { get; set; }
        public DateTime? DeploymentDate { get; set; }
        public int TotalQuestions { get; set; }
        public int ResponseCount { get; set; }
        public double CompletionRate { get; set; }
    }

    /// <summary>
    /// DTO for interview record summary
    /// </summary>
    public class InterviewRecordDto
    {
        public Guid Id { get; set; }
        public DateTime RecordedAt { get; set; }
        public bool HasTranscript { get; set; }
        public bool HasRecording { get; set; }
        public string? FileName { get; set; }
        public long FileSizeBytes { get; set; }
    }
}
