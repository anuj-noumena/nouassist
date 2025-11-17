using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.DocumentManagement.ValueObjects
{
    public class OCRResult : ValueObject
    {
        public string ExtractedText { get; init; }
        public Dictionary<string, string> ExtractedData { get; init; }
        public decimal ConfidenceScore { get; init; }
        public DateTime ProcessedAt { get; init; }
        public string ProcessorVersion { get; init; }

        protected OCRResult() { }

        public OCRResult(
            string extractedText,
            Dictionary<string, string> extractedData,
            decimal confidenceScore,
            DateTime processedAt,
            string processorVersion = "1.0")
        {
            ExtractedText = extractedText ?? string.Empty;
            ExtractedData = extractedData ?? new Dictionary<string, string>();
            ConfidenceScore = Math.Clamp(confidenceScore, 0, 1);
            ProcessedAt = processedAt;
            ProcessorVersion = processorVersion;
        }

        public bool IsHighConfidence => ConfidenceScore >= 0.8m;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ExtractedText;
            yield return ConfidenceScore;
            yield return ProcessedAt;
        }
    }
}
