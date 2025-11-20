namespace NouAssist.RequirementsManagement.Elicitation.ValueObjects
{
    /// <summary>
    /// Value object for user story text in "As a... I want... So that..." format
    /// </summary>
    public class StoryText : ValueObject
    {
        public string Persona { get; private set; }
        public string Goal { get; private set; }
        public string Benefit { get; private set; }

        private StoryText() { }

        public StoryText(string persona, string goal, string benefit)
        {
            Persona = Check.NotNullOrWhiteSpace(persona, nameof(persona));
            Goal = Check.NotNullOrWhiteSpace(goal, nameof(goal));
            Benefit = Check.NotNullOrWhiteSpace(benefit, nameof(benefit));

            var fullText = ToString();
            if (fullText.Length > ElicitationConsts.MaxStoryTextLength)
            {
                throw new BusinessException("Elicitation:Validation:StringLength")
                    .WithData("propertyName", "StoryText")
                    .WithData("min", 0)
                    .WithData("max", ElicitationConsts.MaxStoryTextLength);
            }
        }

        public override string ToString()
        {
            return $"As a {Persona}, I want {Goal}, so that {Benefit}.";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Persona;
            yield return Goal;
            yield return Benefit;
        }
    }
}
