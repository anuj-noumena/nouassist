using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.ValueObjects
{
    /// <summary>
    /// Represents a final ranking score for a requirement
    /// </summary>
    public class RankScore : ValueObject
    {
        public decimal Score { get; private set; }
        public int Rank { get; private set; }
        public decimal Confidence { get; private set; }
        public int VoteCount { get; private set; }

        private RankScore() { }

        public RankScore(decimal score, int rank, decimal confidence, int voteCount)
        {
            if (score < 0)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:InvalidScore");
            }

            if (rank < 1)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:InvalidRank");
            }

            if (confidence < 0 || confidence > 1)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:InvalidConfidence");
            }

            Score = score;
            Rank = rank;
            Confidence = confidence;
            VoteCount = voteCount;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Score;
            yield return Rank;
            yield return Confidence;
            yield return VoteCount;
        }
    }
}
