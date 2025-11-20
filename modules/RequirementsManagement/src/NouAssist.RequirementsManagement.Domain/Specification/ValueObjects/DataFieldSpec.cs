namespace NouAssist.RequirementsManagement.Specification.ValueObjects
{
    /// <summary>
    /// Value object representing a data field specification
    /// </summary>
    public class DataFieldSpec : ValueObject
    {
        public string FieldName { get; private set; }
        public DataFieldType DataType { get; private set; }
        public bool IsRequired { get; private set; }
        public string Description { get; private set; }
        public int? MaxLength { get; private set; }

        protected DataFieldSpec() { }

        public DataFieldSpec(
            string fieldName,
            DataFieldType dataType,
            bool isRequired,
            string description = null,
            int? maxLength = null)
        {
            SetFieldName(fieldName);
            DataType = dataType;
            IsRequired = isRequired;
            SetDescription(description);
            MaxLength = maxLength;
        }

        private void SetFieldName(string fieldName)
        {
            FieldName = Check.NotNullOrWhiteSpace(
                fieldName,
                nameof(fieldName),
                maxLength: RequirementSpecificationManagementConsts.DataRequirement.MaxFieldNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: RequirementSpecificationManagementConsts.DataRequirement.MaxDescriptionLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FieldName;
            yield return DataType;
            yield return IsRequired;
            yield return Description;
            yield return MaxLength;
        }
    }
}
