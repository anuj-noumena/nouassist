using NouAssist.WorkManagement.TaskManagement.Enums;
using NouAssist.WorkManagement.TaskManagement.ValueObjects;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.WorkManagement.TaskManagement
{
    public class ComplexityEstimationTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(8)]
        [InlineData(13)]
        [InlineData(21)]
        [InlineData(34)]
        public void Constructor_WithValidFibonacciValue_Should_CreateComplexityEstimation(int value)
        {
            // Act
            var complexity = new ComplexityEstimation(value, ComplexityScale.Fibonacci);

            // Assert
            complexity.Value.ShouldBe(value);
            complexity.Scale.ShouldBe(ComplexityScale.Fibonacci);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(7)]
        [InlineData(10)]
        public void Constructor_WithInvalidFibonacciValue_Should_ThrowException(int value)
        {
            // Act & Assert
            Should.Throw<BusinessException>(() => new ComplexityEstimation(value, ComplexityScale.Fibonacci))
                .Code.ShouldBe("Tasks:InvalidFibonacciValue");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void Constructor_WithValidTShirtValue_Should_CreateComplexityEstimation(int value)
        {
            // Act
            var complexity = new ComplexityEstimation(value, ComplexityScale.TShirt);

            // Assert
            complexity.Value.ShouldBe(value);
            complexity.Scale.ShouldBe(ComplexityScale.TShirt);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(10)]
        public void Constructor_WithInvalidTShirtValue_Should_ThrowException(int value)
        {
            // Act & Assert
            Should.Throw<BusinessException>(() => new ComplexityEstimation(value, ComplexityScale.TShirt))
                .Code.ShouldBe("Tasks:InvalidTShirtValue");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void Constructor_WithValidNumericValue_Should_CreateComplexityEstimation(int value)
        {
            // Act
            var complexity = new ComplexityEstimation(value, ComplexityScale.Numeric);

            // Assert
            complexity.Value.ShouldBe(value);
            complexity.Scale.ShouldBe(ComplexityScale.Numeric);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(11)]
        [InlineData(20)]
        public void Constructor_WithInvalidNumericValue_Should_ThrowException(int value)
        {
            // Act & Assert
            Should.Throw<BusinessException>(() => new ComplexityEstimation(value, ComplexityScale.Numeric))
                .Code.ShouldBe("Tasks:InvalidNumericValue");
        }

        [Fact]
        public void Unknown_Should_CreateUnknownComplexityEstimation()
        {
            // Act
            var complexity = ComplexityEstimation.Unknown();

            // Assert
            complexity.Value.ShouldBe(0);
            complexity.Scale.ShouldBe(ComplexityScale.Unknown);
        }

        [Theory]
        [InlineData(1, "XS")]
        [InlineData(2, "S")]
        [InlineData(3, "M")]
        [InlineData(4, "L")]
        [InlineData(5, "XL")]
        [InlineData(6, "XXL")]
        public void GetTShirtSize_WithValidValue_Should_ReturnCorrectSize(int value, string expectedSize)
        {
            // Arrange
            var complexity = new ComplexityEstimation(value, ComplexityScale.TShirt);

            // Act
            var size = complexity.GetTShirtSize();

            // Assert
            size.ShouldBe(expectedSize);
        }

        [Fact]
        public void GetTShirtSize_WithNonTShirtScale_Should_ReturnNull()
        {
            // Arrange
            var complexity = new ComplexityEstimation(5, ComplexityScale.Numeric);

            // Act
            var size = complexity.GetTShirtSize();

            // Assert
            size.ShouldBeNull();
        }

        [Theory]
        [InlineData(ComplexityScale.Fibonacci, 1, 0.1)]
        [InlineData(ComplexityScale.Fibonacci, 5, 0.5)]
        [InlineData(ComplexityScale.Fibonacci, 13, 0.85)]
        [InlineData(ComplexityScale.TShirt, 3, 0.5)]
        [InlineData(ComplexityScale.TShirt, 6, 1.0)]
        [InlineData(ComplexityScale.Numeric, 5, 0.5)]
        [InlineData(ComplexityScale.Numeric, 10, 1.0)]
        public void GetNormalizedComplexity_Should_ReturnCorrectValue(ComplexityScale scale, int value, double expected)
        {
            // Arrange
            var complexity = new ComplexityEstimation(value, scale);

            // Act
            var normalized = complexity.GetNormalizedComplexity();

            // Assert
            normalized.ShouldBe(expected, tolerance: 0.01);
        }

        [Fact]
        public void ToString_WithTShirtScale_Should_ReturnSize()
        {
            // Arrange
            var complexity = new ComplexityEstimation(3, ComplexityScale.TShirt);

            // Act
            var result = complexity.ToString();

            // Assert
            result.ShouldBe("M");
        }

        [Fact]
        public void ToString_WithNumericScale_Should_ReturnValue()
        {
            // Arrange
            var complexity = new ComplexityEstimation(5, ComplexityScale.Numeric);

            // Act
            var result = complexity.ToString();

            // Assert
            result.ShouldBe("5");
        }

        [Fact]
        public void Equality_WithSameValues_Should_BeEqual()
        {
            // Arrange
            var complexity1 = new ComplexityEstimation(5, ComplexityScale.Fibonacci);
            var complexity2 = new ComplexityEstimation(5, ComplexityScale.Fibonacci);

            // Act & Assert
            complexity1.Equals(complexity2).ShouldBeTrue();
        }

        [Fact]
        public void Equality_WithDifferentValues_Should_NotBeEqual()
        {
            // Arrange
            var complexity1 = new ComplexityEstimation(5, ComplexityScale.Fibonacci);
            var complexity2 = new ComplexityEstimation(8, ComplexityScale.Fibonacci);

            // Act & Assert
            complexity1.Equals(complexity2).ShouldBeFalse();
        }
    }
}
