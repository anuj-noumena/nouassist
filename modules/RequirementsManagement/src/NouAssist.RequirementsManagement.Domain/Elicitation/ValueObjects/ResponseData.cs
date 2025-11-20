namespace NouAssist.RequirementsManagement.Elicitation.ValueObjects
{
    /// <summary>
    /// Value object for survey response data
    /// </summary>
    public class ResponseData : ValueObject
    {
        public Dictionary<string, object> Answers { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime SubmittedAt { get; private set; }

        private ResponseData() { }

        public ResponseData(Dictionary<string, object> answers, DateTime startedAt, DateTime submittedAt)
        {
            Answers = answers ?? new Dictionary<string, object>();
            StartedAt = startedAt;
            SubmittedAt = submittedAt;

            if (submittedAt < startedAt)
            {
                throw new BusinessException("Elicitation:Validation:InvalidValue")
                    .WithData("propertyName", "SubmittedAt");
            }
        }

        public TimeSpan ResponseTime => SubmittedAt - StartedAt;

        public T GetAnswer<T>(string questionId)
        {
            if (!Answers.ContainsKey(questionId))
                return default(T);

            return (T)Answers[questionId];
        }

        public bool HasAnswer(string questionId)
        {
            return Answers.ContainsKey(questionId) && Answers[questionId] != null;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return StartedAt;
            yield return SubmittedAt;
            foreach (var answer in Answers)
            {
                yield return answer.Key;
                yield return answer.Value?.ToString() ?? string.Empty;
            }
        }
    }
}
