using System;
using System.Threading.Tasks;
using NouAssist.CurrencyManagement.Dtos;
using Shouldly;
using Xunit;

namespace NouAssist.CurrencyManagement
{
    public class ExchangeRateAppServiceTests : CurrencyManagementApplicationTestBase
    {
        private readonly IExchangeRateAppService _exchangeRateAppService;
        private readonly ICurrencyAppService _currencyAppService;

        public ExchangeRateAppServiceTests()
        {
            _exchangeRateAppService = GetRequiredService<IExchangeRateAppService>();
            _currencyAppService = GetRequiredService<ICurrencyAppService>();
        }

        [Fact]
        public async Task Should_Create_Exchange_Rate()
        {
            // Arrange
            await CreateTestCurrenciesAsync();

            var input = new CreateExchangeRateDto
            {
                BaseCurrency = "USD",
                TargetCurrency = "EUR",
                Rate = 0.85m,
                EffectiveDate = DateTime.Today
            };

            // Act
            var result = await _exchangeRateAppService.CreateAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.Rate.ShouldBe(0.85m);
            result.BaseCurrency.ShouldBe("USD");
            result.TargetCurrency.ShouldBe("EUR");
        }

        [Fact]
        public async Task Should_Convert_Currency()
        {
            // Arrange
            await CreateTestCurrenciesAsync();
            await CreateTestExchangeRateAsync();

            var input = new ConvertCurrencyDto
            {
                Amount = 100m,
                FromCurrency = "USD",
                ToCurrency = "EUR"
            };

            // Act
            var result = await _exchangeRateAppService.ConvertAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.ConvertedAmount.ShouldBe(85m);
            result.ExchangeRate.ShouldBe(0.85m);
        }

        private async Task CreateTestCurrenciesAsync()
        {
            await _currencyAppService.CreateAsync(new CreateCurrencyDto
            {
                Code = "USD",
                Name = "US Dollar",
                Symbol = "$",
                DecimalPlaces = 2,
                IsBaseCurrency = true
            });

            await _currencyAppService.CreateAsync(new CreateCurrencyDto
            {
                Code = "EUR",
                Name = "Euro",
                Symbol = "€",
                DecimalPlaces = 2
            });
        }

        private async Task CreateTestExchangeRateAsync()
        {
            await _exchangeRateAppService.CreateAsync(new CreateExchangeRateDto
            {
                BaseCurrency = "USD",
                TargetCurrency = "EUR",
                Rate = 0.85m,
                EffectiveDate = DateTime.Today
            });
        }
    }
}
