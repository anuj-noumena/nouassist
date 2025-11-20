namespace NouAssist.RequirementsManagement.Elicitation.ValueObjects
{
    /// <summary>
    /// Value object for use case definition with actors and steps
    /// </summary>
    public class UseCaseModel : ValueObject
    {
        public string Name { get; private set; }
        public List<string> Actors { get; private set; }
        public List<UseCaseStep> Steps { get; private set; }
        public PrePostCondition PreCondition { get; private set; }
        public PrePostCondition PostCondition { get; private set; }

        private UseCaseModel() { }

        public UseCaseModel(
            string name,
            List<string> actors,
            List<UseCaseStep> steps,
            PrePostCondition preCondition = null,
            PrePostCondition postCondition = null)
        {
            SetName(name);
            Actors = actors ?? new List<string>();
            Steps = steps ?? new List<UseCaseStep>();
            PreCondition = preCondition;
            PostCondition = postCondition;

            if (Actors.Count == 0)
            {
                throw new BusinessException("Elicitation:Validation:Required")
                    .WithData("propertyName", "Actors");
            }

            if (Steps.Count == 0)
            {
                throw new BusinessException("Elicitation:Validation:Required")
                    .WithData("propertyName", "Steps");
            }
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ElicitationConsts.MaxUseCaseNameLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            foreach (var actor in Actors)
            {
                yield return actor;
            }
            foreach (var step in Steps)
            {
                yield return step;
            }
            yield return PreCondition?.ToString() ?? string.Empty;
            yield return PostCondition?.ToString() ?? string.Empty;
        }
    }

    public class UseCaseStep : ValueObject
    {
        public int StepNumber { get; private set; }
        public string Actor { get; private set; }
        public string Description { get; private set; }

        private UseCaseStep() { }

        public UseCaseStep(int stepNumber, string actor, string description)
        {
            StepNumber = stepNumber;
            Actor = Check.NotNullOrWhiteSpace(actor, nameof(actor), maxLength: ElicitationConsts.MaxActorNameLength);
            SetDescription(description);
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: ElicitationConsts.MaxStepDescriptionLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return StepNumber;
            yield return Actor;
            yield return Description;
        }
    }
}
