using System;
using System.Threading.Tasks;
using NouAssist.CurrencyManagement.Aggregates;
using NouAssist.CurrencyManagement.Data;
using NouAssist.CurrencyManagement.Enums;
using NouAssist.CurrencyManagement.Services;
using NouAssist.CurrencyManagement.ValueObjects;
using NSubstitute;
using Shouldly;
using Xunit;

namespace NouAssist.CurrencyManagement.Tests.Services
{
    public class CurrencyConversionServiceTests
    {
        private readonly ICurrencyExchangeRateRepository _exchangeRateRepository;
        private readonly CurrencyConversionService _conversionService;

        public CurrencyConversionServiceTests()
        {
            _exchangeRateRepository = Substitute.For<ICurrencyExchangeRateRepository>();
            _conversionService = new CurrencyConversionService(_exchangeRateRepository);
        }

        [Fact]
        public async Task ConvertAsync_SameCurrency_Should_ReturnOriginalAmount()
        {
            // Arrange
            var amount = new MonetaryAmount(100m, new CurrencyCode("USD"));

            // Act
            var result = await _conversionService.ConvertAsync(amount, new CurrencyCode("USD"));

            // Assert
            result.Amount.ShouldBe(100m);
            result.Currency.Code.ShouldBe("USD");
        }

        [Fact]
        public async Task ConvertAsync_Should_ConvertUsingExchangeRate()
        {
            // Arrange
            var usd = new CurrencyCode("USD");
            var eur = new CurrencyCode("EUR");
            var amount = new MonetaryAmount(100m, usd);
            var effectiveDate = new DateTime(2025, 1, 1);

            var exchangeRate = new CurrencyExchangeRate(
                Guid.NewGuid(),
                null,
                usd,
                eur,
                0.85m, // 1 USD = 0.85 EUR
                effectiveDate,
                ExchangeRateProvider.Manual);

            _exchangeRateRepository
                .GetRateAsync(usd, eur, Arg.Any<DateTime>())
                .Returns(exchangeRate);

            // Act
            var result = await _conversionService.ConvertAsync(amount, eur, effectiveDate);

            // Assert
            result.Amount.ShouldBe(85m); // 100 * 0.85
            result.Currency.Code.ShouldBe("EUR");
        }

        [Fact]
        public async Task GetExchangeRateAsync_Should_ReturnRate()
        {
            // Arrange
            var usd = new CurrencyCode("USD");
            var gbp = new CurrencyCode("GBP");
            var effectiveDate = new DateTime(2025, 1, 1);

            var exchangeRate = new CurrencyExchangeRate(
                Guid.NewGuid(),
                null,
                usd,
                gbp,
                0.75m,
                effectiveDate,
                ExchangeRateProvider.CentralBank);

            _exchangeRateRepository
                .GetRateAsync(usd, gbp, effectiveDate)
                .Returns(exchangeRate);

            // Act
            var result = await _conversionService.GetExchangeRateAsync(usd, gbp, effectiveDate);

            // Assert
            result.ShouldBe(0.75m);
        }
    }
}
