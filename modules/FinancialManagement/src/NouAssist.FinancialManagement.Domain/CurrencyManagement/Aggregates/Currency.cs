using System;
using NouAssist.CurrencyManagement.Domain.Shared.Events;
using NouAssist.CurrencyManagement.Enums;
using NouAssist.FinancialManagement.CurrencyManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.CurrencyManagement.Aggregates
{
    public class Currency : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public CurrencyCode Code { get; private set; }
        public string Name { get; private set; }
        public string Symbol { get; private set; }
        public int DecimalPlaces { get; private set; }
        public CurrencyStatus Status { get; private set; }
        public bool IsBaseCurrency { get; private set; }
        public int DisplayOrder { get; set; }

        protected Currency() { }

        public Currency(
            Guid id,
            Guid? tenantId,
            CurrencyCode code,
            string name,
            string symbol,
            int decimalPlaces = 2,
            bool isBaseCurrency = false) : base(id)
        {
            TenantId = tenantId;
            Code = Check.NotNull(code, nameof(code));
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Symbol = Check.NotNullOrWhiteSpace(symbol, nameof(symbol));
            DecimalPlaces = decimalPlaces;
            Status = CurrencyStatus.Active;
            IsBaseCurrency = isBaseCurrency;
            DisplayOrder = 0;
        }

        public void Activate()
        {
            if (Status == CurrencyStatus.Active)
            {
                return;
            }

            Status = CurrencyStatus.Active;

            AddDistributedEvent(new CurrencyActivatedEto
            {
                CurrencyId = Id,
                TenantId = TenantId,
                CurrencyCode = Code.Code,
                CurrencyName = Name,
                ActivatedAt = DateTime.UtcNow
            });
        }

        public void Deactivate()
        {
            if (IsBaseCurrency)
            {
                throw new BusinessException("CurrencyManagement:CannotDeactivateBaseCurrency")
                    .WithData("CurrencyCode", Code.Code);
            }

            Status = CurrencyStatus.Inactive;
        }

        public void Deprecate()
        {
            if (IsBaseCurrency)
            {
                throw new BusinessException("CurrencyManagement:CannotDeactivateBaseCurrency")
                    .WithData("CurrencyCode", Code.Code);
            }

            Status = CurrencyStatus.Deprecated;
        }

        public void SetAsBaseCurrency()
        {
            IsBaseCurrency = true;
            Status = CurrencyStatus.Active;
        }
    }
}
