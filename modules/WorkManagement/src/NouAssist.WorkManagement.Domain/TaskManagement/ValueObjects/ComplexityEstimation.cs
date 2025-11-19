using System.Collections.Generic;
using NouAssist.WorkManagement.TaskManagement.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.TaskManagement.ValueObjects
{
    public class ComplexityEstimation : ValueObject
    {
        public int Value { get; private set; }
        public ComplexityScale Scale { get; private set; }

        private ComplexityEstimation() { }

        public ComplexityEstimation(int value, ComplexityScale scale)
        {
            ValidateValue(value, scale);
            Value = value;
            Scale = scale;
        }

        private static void ValidateValue(int value, ComplexityScale scale)
        {
            switch (scale)
            {
                case ComplexityScale.Fibonacci:
                    var validFibonacci = new[] { 1, 2, 3, 5, 8, 13, 21, 34, 55, 89 };
                    if (!System.Array.Exists(validFibonacci, x => x == value))
                    {
                        throw new BusinessException("Tasks:InvalidFibonacciValue",
                            $"Value {value} is not a valid Fibonacci number. Valid values: 1, 2, 3, 5, 8, 13, 21, 34, 55, 89");
                    }
                    break;

                case ComplexityScale.TShirt:
                    if (value < 1 || value > 6)
                    {
                        throw new BusinessException("Tasks:InvalidTShirtValue",
                            $"T-shirt size value must be between 1 (XS) and 6 (XXL). Got: {value}");
                    }
                    break;

                case ComplexityScale.Numeric:
                    if (value < 1 || value > 10)
                    {
                        throw new BusinessException("Tasks:InvalidNumericValue",
                            $"Numeric complexity value must be between 1 and 10. Got: {value}");
                    }
                    break;

                case ComplexityScale.Unknown:
                    // No validation needed for unknown scale
                    break;

                default:
                    throw new BusinessException("Tasks:InvalidComplexityScale",
                        $"Unknown complexity scale: {scale}");
            }
        }

        /// <summary>
        /// Creates an Unknown complexity estimation
        /// </summary>
        public static ComplexityEstimation Unknown()
        {
            return new ComplexityEstimation(0, ComplexityScale.Unknown);
        }

        /// <summary>
        /// Gets the display name for T-shirt sizing
        /// </summary>
        public string GetTShirtSize()
        {
            if (Scale != ComplexityScale.TShirt)
            {
                return null;
            }

            return Value switch
            {
                1 => "XS",
                2 => "S",
                3 => "M",
                4 => "L",
                5 => "XL",
                6 => "XXL",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Gets a normalized complexity value (0.0 to 1.0) for comparison across different scales
        /// </summary>
        public double GetNormalizedComplexity()
        {
            return Scale switch
            {
                ComplexityScale.Fibonacci => Value switch
                {
                    1 => 0.1,
                    2 => 0.2,
                    3 => 0.3,
                    5 => 0.5,
                    8 => 0.7,
                    13 => 0.85,
                    21 => 0.95,
                    _ => 1.0
                },
                ComplexityScale.TShirt => Value / 6.0,
                ComplexityScale.Numeric => Value / 10.0,
                ComplexityScale.Unknown => 0.0,
                _ => 0.0
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Scale;
        }

        public override string ToString()
        {
            return Scale switch
            {
                ComplexityScale.TShirt => GetTShirtSize(),
                ComplexityScale.Unknown => "Unknown",
                _ => Value.ToString()
            };
        }
    }
}