namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects
{
    /// <summary>
    /// Value object representing the relationship score with an investor
    /// </summary>
    public class RelationshipScore : ValueObject
    {
        public int Score { get; private set; }
        public DateTime LastCalculated { get; private set; }
        public string Notes { get; private set; }

        private RelationshipScore() { }

        public RelationshipScore(int score, DateTime lastCalculated, string notes = null)
        {
            if (score < 0 || score > 100)
                throw new BusinessException("IRM:ErrorMessage:InvalidOperation", "Relationship score must be between 0 and 100");

            Score = score;
            LastCalculated = lastCalculated;
            Notes = notes;
        }

        public RelationshipScore UpdateScore(int newScore, string notes = null)
        {
            return new RelationshipScore(newScore, DateTime.UtcNow, notes);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Score;
            yield return LastCalculated;
            yield return Notes;
        }
    }
}
