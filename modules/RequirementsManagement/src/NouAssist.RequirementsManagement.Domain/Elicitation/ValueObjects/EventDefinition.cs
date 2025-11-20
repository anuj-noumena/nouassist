namespace NouAssist.RequirementsManagement.Elicitation.ValueObjects
{
    /// <summary>
    /// Value object for event definition in elicitation artifacts
    /// </summary>
    public class EventDefinition : ValueObject
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Trigger { get; private set; }
        public List<string> Actors { get; private set; }
        public Dictionary<string, string> Properties { get; private set; }

        private EventDefinition() { }

        public EventDefinition(
            string name,
            string description,
            string trigger,
            List<string> actors = null,
            Dictionary<string, string> properties = null)
        {
            SetName(name);
            SetDescription(description);
            Trigger = Check.NotNullOrWhiteSpace(trigger, nameof(trigger));
            Actors = actors ?? new List<string>();
            Properties = properties ?? new Dictionary<string, string>();
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ElicitationConsts.MaxEventNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: ElicitationConsts.MaxEventDescriptionLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Description;
            yield return Trigger;
            foreach (var actor in Actors)
            {
                yield return actor;
            }
            foreach (var property in Properties)
            {
                yield return property.Key;
                yield return property.Value;
            }
        }
    }
}
