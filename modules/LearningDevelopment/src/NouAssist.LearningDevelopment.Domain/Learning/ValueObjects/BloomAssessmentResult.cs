using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.Learning.ValueObjects
{
    /// <summary>
    /// Represents Bloom's Taxonomy assessment results
    /// </summary>
    public class BloomAssessmentResult : ValueObject
    {
        public double RememberScore { get; init; }
        public double UnderstandScore { get; init; }
        public double ApplyScore { get; init; }
        public double AnalyzeScore { get; init; }
        public double EvaluateScore { get; init; }
        public double CreateScore { get; init; }
        public double OverallScore { get; init; }

        public BloomAssessmentResult(
            double remember, double understand, double apply,
            double analyze, double evaluate, double create)
        {
            RememberScore = remember;
            UnderstandScore = understand;
            ApplyScore = apply;
            AnalyzeScore = analyze;
            EvaluateScore = evaluate;
            CreateScore = create;
            OverallScore = (remember + understand + apply + analyze + evaluate + create) / 6.0;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return RememberScore;
            yield return UnderstandScore;
            yield return ApplyScore;
            yield return AnalyzeScore;
            yield return EvaluateScore;
            yield return CreateScore;
            yield return OverallScore;
        }
    }
}
