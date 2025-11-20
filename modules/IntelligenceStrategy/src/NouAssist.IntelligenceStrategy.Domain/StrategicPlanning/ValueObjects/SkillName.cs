namespace NouAssist.IntelligenceStrategy.StrategicPlanning.ValueObjects
{
    /// <summary>
    /// Value object representing a skill or capability name
    /// </summary>
    public class SkillName : ValueObject
    {
        public string Name { get; private set; }
        public string Category { get; private set; }
        public string Description { get; private set; }

        private SkillName() { }

        public SkillName(string name, string category, string description = null)
        {
            SetName(name);
            SetCategory(category);
            Description = description;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 100);
        }

        private void SetCategory(string category)
        {
            Category = Check.NotNullOrWhiteSpace(category, nameof(category), maxLength: 50);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Category;
            yield return Description;
        }
    }
}
