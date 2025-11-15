using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.Learning.ValueObjects
{
    /// <summary>
    /// Represents progress in a single spaced repetition cycle
    /// </summary>
    public class CycleProgress : ValueObject
    {
        public int CycleNumber { get; private set; }
        public bool Completed { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public double? RecallAccuracy { get; private set; }
        public double? AverageConfidence { get; private set; }
        public int? CompletionRate { get; private set; }
        public Dictionary<string, bool> Improvements { get; private set; }
        public Dictionary<string, string> Reflection { get; private set; }

        private CycleProgress() { /* ORM */ }

        public CycleProgress(int cycleNumber)
        {
            if (cycleNumber < 1 || cycleNumber > 4)
            {
                throw new ArgumentException("Cycle number must be between 1 and 4", nameof(cycleNumber));
            }

            CycleNumber = cycleNumber;
            Completed = false;
            Improvements = new Dictionary<string, bool>();
            Reflection = new Dictionary<string, string>();
        }

        public CycleProgress Complete(
            double recallAccuracy,
            double averageConfidence,
            int completionRate,
            Dictionary<string, bool> improvements,
            Dictionary<string, string> reflection)
        {
            return new CycleProgress
            {
                CycleNumber = CycleNumber,
                Completed = true,
                CompletedAt = DateTime.UtcNow,
                RecallAccuracy = recallAccuracy,
                AverageConfidence = averageConfidence,
                CompletionRate = completionRate,
                Improvements = improvements ?? new Dictionary<string, bool>(),
                Reflection = reflection ?? new Dictionary<string, string>()
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return CycleNumber;
            yield return Completed;
            yield return CompletedAt;
            yield return RecallAccuracy;
            yield return AverageConfidence;
            yield return CompletionRate;
        }
    }
}
