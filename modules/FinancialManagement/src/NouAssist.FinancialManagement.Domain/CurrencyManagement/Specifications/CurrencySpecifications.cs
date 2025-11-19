using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.CurrencyManagement.Aggregates;
using NouAssist.FinancialManagement.CurrencyManagement.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.CurrencyManagement.Specifications
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
