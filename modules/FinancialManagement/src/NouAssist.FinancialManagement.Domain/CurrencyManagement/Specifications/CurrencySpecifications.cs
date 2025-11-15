using System;
using System.Linq.Expressions;
using NouAssist.CurrencyManagement.Aggregates;
using NouAssist.CurrencyManagement.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.CurrencyManagement.Specifications
{
    public class ActiveCurrenciesSpecification : Specification<Currency>
    {
        public override Expression<Func<Currency, bool>> ToExpression()
        {
            return c => c.Status == CurrencyStatus.Active;
        }
    }

    public class BaseCurrencySpecification : Specification<Currency>
    {
        public override Expression<Func<Currency, bool>> ToExpression()
        {
            return c => c.IsBaseCurrency;
        }
    }
}
