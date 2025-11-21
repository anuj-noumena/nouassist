using System;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.Elicitation.Entities
{
    /// <summary>
    /// Entity for interview records including transcripts and recordings
    /// </summary>
    public class InterviewRecord : Entity<Guid>
    {
        public Guid SessionId { get; private set; }
        public TranscriptRecordingRef TranscriptRecording { get; private set; }
        public DateTime RecordedAt { get; private set; }

        protected InterviewRecord() { }

        internal InterviewRecord(Guid sessionId, TranscriptRecordingRef transcriptRecording)
        {
            Id = Guid.NewGuid();
            SessionId = sessionId;
            TranscriptRecording = Check.NotNull(transcriptRecording, nameof(transcriptRecording));
            RecordedAt = DateTime.UtcNow;
        }

        public void UpdateTranscriptRecording(TranscriptRecordingRef transcriptRecording)
        {
            TranscriptRecording = Check.NotNull(transcriptRecording, nameof(transcriptRecording));
        }

        public bool HasTranscript => TranscriptRecording.HasTranscript;
        public bool HasRecording => TranscriptRecording.HasRecording;
    }
}
