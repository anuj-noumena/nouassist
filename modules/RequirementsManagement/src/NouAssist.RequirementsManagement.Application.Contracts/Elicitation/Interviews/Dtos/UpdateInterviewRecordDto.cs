using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Elicitation.Constants;

namespace NouAssist.RequirementsManagement.Elicitation.Interviews.Dtos
{
    /// <summary>
    /// DTO for updating interview record
    /// </summary>
    public class UpdateInterviewRecordDto
    {
        [StringLength(ElicitationConsts.MaxTranscriptLength)]
        public string? TranscriptText { get; set; }

        [StringLength(ElicitationConsts.MaxRecordingPathLength)]
        public string? RecordingPath { get; set; }

        [StringLength(ElicitationConsts.MaxFileNameLength)]
        public string? FileName { get; set; }

        public long FileSizeBytes { get; set; }

        [StringLength(100)]
        public string? MimeType { get; set; }
    }
}
