using NouAssist.FinancialManagement.CurrencyManagement.Aggregates;
using NouAssist.FinancialManagement.CurrencyManagement.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.FinancialManagement.CurrencyManagement
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CurrencyToCurrencyDtoMapper : MapperBase<Currency, CurrencyDto>
    {
        [MapperIgnoreSource(nameof(Currency.TenantId))]
        [MapperIgnoreTarget(nameof(CurrencyDto.Code))]
        public override partial CurrencyDto Map(Currency source);

        [MapperIgnoreSource(nameof(Currency.TenantId))]
        [MapperIgnoreTarget(nameof(CurrencyDto.Code))]
        public override partial void Map(Currency source, CurrencyDto destination);

        public override void AfterMap(Currency source, CurrencyDto destination)
        {
            destination.Code = source.Code.Code;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CurrencyExchangeRateToExchangeRateDtoMapper : MapperBase<CurrencyExchangeRate, ExchangeRateDto>
    {
        [MapperIgnoreTarget(nameof(ExchangeRateDto.Rate))]
        [MapperIgnoreTarget(nameof(ExchangeRateDto.InverseRate))]
        [MapperIgnoreTarget(nameof(ExchangeRateDto.EffectiveDate))]
        public override partial ExchangeRateDto Map(CurrencyExchangeRate source);

        [MapperIgnoreTarget(nameof(ExchangeRateDto.Rate))]
        [MapperIgnoreTarget(nameof(ExchangeRateDto.InverseRate))]
        [MapperIgnoreTarget(nameof(ExchangeRateDto.EffectiveDate))]
        public override partial void Map(CurrencyExchangeRate source, ExchangeRateDto destination);

        public override void AfterMap(CurrencyExchangeRate source, ExchangeRateDto destination)
        {
            destination.BaseCurrency = source.BaseCurrency.Code;
            destination.TargetCurrency = source.TargetCurrency.Code;
            destination.Rate = source.Rate.Rate;
            destination.InverseRate = source.Rate.InverseRate;
            destination.EffectiveDate = source.Rate.EffectiveDate;
        }
    }
}
