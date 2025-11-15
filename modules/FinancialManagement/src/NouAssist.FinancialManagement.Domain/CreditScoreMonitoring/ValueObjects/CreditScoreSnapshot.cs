using System;
using System.Collections.Generic;
using NouAssist.CreditScoreMonitoring.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.CreditScoreMonitoring.ValueObjects
{
    public class CreditScoreSnapshot : ValueObject
    {
        public int Score { get; init; }
        public CreditScoreRange Range { get; init; }
        public DateTime SnapshotDate { get; init; }
        public CreditBureau Bureau { get; init; }
        public int? ChangeFromPrevious { get; init; }

        public CreditScoreSnapshot(
            int score,
            CreditScoreRange range,
            DateTime snapshotDate,
            CreditBureau bureau,
            int? changeFromPrevious = null)
        {
            Score = score;
            Range = range;
            SnapshotDate = snapshotDate;
            Bureau = bureau;
            ChangeFromPrevious = changeFromPrevious;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Score;
            yield return Range;
            yield return SnapshotDate;
            yield return Bureau;
            yield return ChangeFromPrevious ?? 0;
        }
    }
}
