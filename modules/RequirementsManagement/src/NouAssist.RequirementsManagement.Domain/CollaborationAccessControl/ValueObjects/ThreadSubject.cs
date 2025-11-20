namespace NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects
{
    /// <summary>
    /// Represents the subject of a discussion thread.
    /// </summary>
    public class ThreadSubject : ValueObject
    {
        public string Value { get; private set; }

        private ThreadSubject() { }

        public ThreadSubject(string value)
        {
            SetValue(value);
        }

        private void SetValue(string value)
        {
            Value = Check.NotNullOrWhiteSpace(
                value,
                nameof(value),
                maxLength: CollaborationConsts.MaxThreadSubjectLength
            );
        }

        public static implicit operator string(ThreadSubject subject) => subject.Value;
        public static explicit operator ThreadSubject(string subject) => new ThreadSubject(subject);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}
