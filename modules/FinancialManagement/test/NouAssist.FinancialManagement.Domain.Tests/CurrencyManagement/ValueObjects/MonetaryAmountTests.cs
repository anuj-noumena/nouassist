using NouAssist.CurrencyManagement.ValueObjects;
using Shouldly;
using Xunit;

namespace NouAssist.CurrencyManagement.Tests.ValueObjects
{
    public class MonetaryAmountTests
    {
        [Fact]
        public void Add_SameCurrency_Should_AddAmounts()
        {
            // Arrange
            var amount1 = new MonetaryAmount(100m, new CurrencyCode("USD"));
            var amount2 = new MonetaryAmount(50m, new CurrencyCode("USD"));

            // Act
            var result = amount1.Add(amount2);

            // Assert
            result.Amount.ShouldBe(150m);
            result.Currency.Code.ShouldBe("USD");
        }

        [Fact]
        public void Add_DifferentCurrencies_Should_ThrowException()
        {
            // Arrange
            var usdAmount = new MonetaryAmount(100m, new CurrencyCode("USD"));
            var eurAmount = new MonetaryAmount(50m, new CurrencyCode("EUR"));

            // Act & Assert
            Should.Throw<InvalidOperationException>(() => usdAmount.Add(eurAmount));
        }

        [Fact]
        public void Multiply_Should_MultiplyAmount()
        {
            // Arrange
            var amount = new MonetaryAmount(100m, new CurrencyCode("USD"));

            // Act
            var result = amount.Multiply(1.5m);

            // Assert
            result.Amount.ShouldBe(150m);
            result.Currency.Code.ShouldBe("USD");
        }

        [Fact]
        public void ToString_Should_FormatCorrectly()
        {
            // Arrange
            var amount = new MonetaryAmount(1234.56m, new CurrencyCode("EUR"));

            // Act
            var result = amount.ToString();

            // Assert
            result.ShouldBe("1234.56 EUR");
        }
    }
}
