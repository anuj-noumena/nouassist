namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects
{
    /// <summary>
    /// Value object representing a numerical rating with context
    /// </summary>
    public class FeedbackRating : ValueObject
    {
        public int Value { get; private set; }
        public int MaxValue { get; private set; }
        public string Category { get; private set; }
        public string Comment { get; private set; }

        private FeedbackRating() { }

        public FeedbackRating(int value, int maxValue = 5, string category = "Overall", string comment = null)
        {
            SetMaxValue(maxValue);
            SetValue(value);
            SetCategory(category);
            Comment = comment;
        }

        private void SetValue(int value)
        {
            if (value < 0 || value > MaxValue)
            {
                throw new BusinessException("ProductDiscovery:Validation:InvalidRange", 
                    $"Rating value must be between 0 and {MaxValue}");
            }

            Value = value;
        }

        private void SetMaxValue(int maxValue)
        {
            if (maxValue <= 0)
            {
                throw new BusinessException("ProductDiscovery:Validation:InvalidValue", 
                    "Max value must be greater than 0");
            }

            MaxValue = maxValue;
        }

        private void SetCategory(string category)
        {
            Category = Check.NotNullOrWhiteSpace(
                category,
                nameof(category),
                maxLength: 50
            );
        }

        public double PercentageScore()
        {
            return (double)Value / MaxValue * 100;
        }

        public string GetRatingLevel()
        {
            var percentage = PercentageScore();
            return percentage switch
            {
                >= 80 => "Excellent",
                >= 60 => "Good",
                >= 40 => "Average",
                >= 20 => "Poor",
                _ => "Very Poor"
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return MaxValue;
            yield return Category;
            yield return Comment ?? string.Empty;
        }
    }
}
