namespace NouAssist.RequirementsManagement.AnalysisPrioritization.ValueObjects
{
    /// <summary>
    /// Defines the structure and constraints of a requirement attribute
    /// </summary>
    public class AttributeDefinition : ValueObject
    {
        public string Name { get; private set; }
        public AttributeType Type { get; private set; }
        public string Description { get; private set; }
        public bool IsRequired { get; private set; }
        public string? AllowedValues { get; private set; } // JSON array for enum-like values
        public decimal? MinValue { get; private set; }
        public decimal? MaxValue { get; private set; }

        private AttributeDefinition() { }

        public AttributeDefinition(
            string name,
            AttributeType type,
            string description,
            bool isRequired = false,
            string? allowedValues = null,
            decimal? minValue = null,
            decimal? maxValue = null)
        {
            SetName(name);
            Type = type;
            SetDescription(description);
            IsRequired = isRequired;
            AllowedValues = allowedValues;
            SetValueRange(minValue, maxValue);
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: AnalysisConsts.MaxAttributeNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: AnalysisConsts.MaxAttributeDescriptionLength
            );
        }

        private void SetValueRange(decimal? minValue, decimal? maxValue)
        {
            if (minValue.HasValue && maxValue.HasValue && minValue > maxValue)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:InvalidValueRange");
            }

            MinValue = minValue;
            MaxValue = maxValue;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Type;
            yield return Description;
            yield return IsRequired;
            yield return AllowedValues ?? string.Empty;
            yield return MinValue ?? 0;
            yield return MaxValue ?? 0;
        }
    }
}
