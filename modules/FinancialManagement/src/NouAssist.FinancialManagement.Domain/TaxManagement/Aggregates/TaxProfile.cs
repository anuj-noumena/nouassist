using System;
using System.Collections.Generic;
using NouAssist.TaxManagement.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.TaxManagement.Aggregates
{
    public class TaxProfile : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid? TenantId { get; private set; }
        public TaxFilingStatus DefaultFilingStatus { get; private set; }
        public int NumberOfDependents { get; private set; }
        public bool IsItemizer { get; private set; }
        public bool IsSelfEmployed { get; private set; }
        public string? TaxPreparer { get; private set; }
        public Dictionary<string, string> TaxSettings { get; private set; }

        protected TaxProfile() { /* ORM */ }

        public TaxProfile(
            Guid id,
            Guid userId,
            Guid? tenantId,
            TaxFilingStatus defaultFilingStatus)
            : base(id)
        {
            UserId = userId;
            TenantId = tenantId;
            DefaultFilingStatus = defaultFilingStatus;
            NumberOfDependents = 0;
            IsItemizer = false;
            IsSelfEmployed = false;
            TaxSettings = new Dictionary<string, string>();
        }

        public void UpdateFilingStatus(TaxFilingStatus newStatus)
        {
            DefaultFilingStatus = newStatus;
        }

        public void UpdateDependents(int numberOfDependents)
        {
            NumberOfDependents = Check.NotNegative(numberOfDependents, nameof(numberOfDependents));
        }

        public void UpdateSettings(bool isItemizer, bool isSelfEmployed)
        {
            IsItemizer = isItemizer;
            IsSelfEmployed = isSelfEmployed;
        }

        public void SetTaxPreparer(string preparerName)
        {
            TaxPreparer = Check.NotNullOrWhiteSpace(preparerName, nameof(preparerName));
        }
    }
}
