using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.CurrencyManagement.Aggregates;
using NouAssist.FinancialManagement.CurrencyManagement.ValueObjects;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.CurrencyManagement.Specifications
{
    public class ActiveExchangeRatesSpecification : Specification<CurrencyExchangeRate>
    {
        public override Expression<Func<CurrencyExchangeRate, bool>> ToExpression()
        {
            return rate => rate.IsActive;
        }
    }

    public class ExchangeRateForCurrencyPairSpecification : Specification<CurrencyExchangeRate>
    {
        private readonly CurrencyCode _baseCurrency;
        private readonly CurrencyCode _targetCurrency;

        public ExchangeRateForCurrencyPairSpecification(
            CurrencyCode baseCurrency,
            CurrencyCode targetCurrency)
        {
            _baseCurrency = baseCurrency;
            _targetCurrency = targetCurrency;
        }

        public override Expression<Func<CurrencyExchangeRate, bool>> ToExpression()
        {
            return rate => 
                rate.BaseCurrency.Code == _baseCurrency.Code &&
                rate.TargetCurrency.Code == _targetCurrency.Code;
        }
    }

    public class ExchangeRateValidOnDateSpecification : Specification<CurrencyExchangeRate>
    {
        private readonly DateTime _date;

        public ExchangeRateValidOnDateSpecification(DateTime date)
        {
            _date = date.Date;
        }

        public override Expression<Func<CurrencyExchangeRate, bool>> ToExpression()
        {
            return rate =>
                rate.IsActive &&
                rate.ValidFrom.Date <= _date &&
                (!rate.ValidTo.HasValue || rate.ValidTo.Value.Date >= _date);
        }
    }
}
