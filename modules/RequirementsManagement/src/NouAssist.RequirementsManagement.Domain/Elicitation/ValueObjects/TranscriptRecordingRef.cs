using System.Collections.Generic;
using NouAssist.RequirementsManagement.Elicitation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.Elicitation.ValueObjects
{
    /// <summary>
    /// Value object for interview transcript and recording references
    /// </summary>
    public class TranscriptRecordingRef : ValueObject
    {
        public string TranscriptText { get; private set; }
        public string RecordingPath { get; private set; }
        public string FileName { get; private set; }
        public long FileSizeBytes { get; private set; }
        public string MimeType { get; private set; }

        private TranscriptRecordingRef() { }

        public TranscriptRecordingRef(
            string transcriptText = null,
            string recordingPath = null,
            string fileName = null,
            long fileSizeBytes = 0,
            string mimeType = null)
        {
            SetTranscriptText(transcriptText);
            SetRecordingPath(recordingPath);
            SetFileName(fileName);
            FileSizeBytes = fileSizeBytes;
            MimeType = mimeType ?? string.Empty;
        }

        private void SetTranscriptText(string text)
        {
            if (!string.IsNullOrEmpty(text) && text.Length > ElicitationConsts.MaxTranscriptLength)
            {
                throw new BusinessException("Elicitation:Validation:StringLength")
                    .WithData("propertyName", "TranscriptText")
                    .WithData("min", 0)
                    .WithData("max", ElicitationConsts.MaxTranscriptLength);
            }

            TranscriptText = text ?? string.Empty;
        }

        private void SetRecordingPath(string path)
        {
            if (!string.IsNullOrEmpty(path) && path.Length > ElicitationConsts.MaxRecordingPathLength)
            {
                throw new BusinessException("Elicitation:Validation:StringLength")
                    .WithData("propertyName", "RecordingPath")
                    .WithData("min", 0)
                    .WithData("max", ElicitationConsts.MaxRecordingPathLength);
            }

            RecordingPath = path ?? string.Empty;
        }

        private void SetFileName(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && fileName.Length > ElicitationConsts.MaxFileNameLength)
            {
                throw new BusinessException("Elicitation:Validation:StringLength")
                    .WithData("propertyName", "FileName")
                    .WithData("min", 0)
                    .WithData("max", ElicitationConsts.MaxFileNameLength);
            }

            FileName = fileName ?? string.Empty;
        }

        public bool HasTranscript => !string.IsNullOrEmpty(TranscriptText);
        public bool HasRecording => !string.IsNullOrEmpty(RecordingPath);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TranscriptText ?? string.Empty;
            yield return RecordingPath ?? string.Empty;
            yield return FileName ?? string.Empty;
            yield return FileSizeBytes;
            yield return MimeType ?? string.Empty;
        }
    }
}
