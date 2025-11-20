namespace NouAssist.RequirementsManagement.Elicitation.ValueObjects
{
    /// <summary>
    /// Value object for use case pre/post conditions
    /// </summary>
    public class PrePostCondition : ValueObject
    {
        public string Description { get; private set; }
        public bool IsMandatory { get; private set; }

        private PrePostCondition() { }

        public PrePostCondition(string description, bool isMandatory = true)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            IsMandatory = isMandatory;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Description;
            yield return IsMandatory;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
