using System.Threading.Tasks;
using NouAssist.CurrencyManagement.Dtos;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.CurrencyManagement
{
    public class CurrencyAppServiceTests : CurrencyManagementApplicationTestBase
    {
        private readonly ICurrencyAppService _currencyAppService;

        public CurrencyAppServiceTests()
        {
            _currencyAppService = GetRequiredService<ICurrencyAppService>();
        }

        [Fact]
        public async Task Should_Create_Valid_Currency()
        {
            // Arrange
            var input = new CreateCurrencyDto
            {
                Code = "TST",
                Name = "Test Currency",
                Symbol = "T$",
                DecimalPlaces = 2
            };

            // Act
            var result = await _currencyAppService.CreateAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.Code.ShouldBe("TST");
            result.Name.ShouldBe("Test Currency");
        }

        [Fact]
        public async Task Should_Not_Create_Duplicate_Currency()
        {
            // Arrange
            var input = new CreateCurrencyDto
            {
                Code = "USD",
                Name = "US Dollar",
                Symbol = "$",
                DecimalPlaces = 2
            };

            await _currencyAppService.CreateAsync(input);

            // Act & Assert
            await Should.ThrowAsync<BusinessException>(async () =>
                await _currencyAppService.CreateAsync(input));
        }

        [Fact]
        public async Task Should_Get_Active_Currencies()
        {
            // Act
            var result = await _currencyAppService.GetActiveCurrenciesAsync();

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldNotBeEmpty();
        }
    }
}
