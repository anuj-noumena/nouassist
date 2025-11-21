using System;
using Volo.Abp.Domain.Entities.Auditing;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using Volo.Abp;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Entities
{
    /// <summary>
    /// Tracks the valuation history of the company
    /// </summary>
    public class ValuationHistory : CreationAuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public Guid CapTableId { get; private set; }
        public DateTime ValuationDate { get; private set; }
        public Valuation Valuation { get; private set; }
        public string Source { get; private set; }
        public string Notes { get; private set; }
        public Guid? FundingRoundId { get; private set; }

        protected ValuationHistory() { }

        internal ValuationHistory(
            Guid id,
            Guid capTableId,
            DateTime valuationDate,
            Valuation valuation,
            string source,
            string notes = null,
            Guid? fundingRoundId = null,
            Guid? tenantId = null) : base(id)
        {
            CapTableId = capTableId;
            ValuationDate = valuationDate;
            Valuation = valuation ?? throw new ArgumentNullException(nameof(valuation));
            Source = Check.NotNullOrWhiteSpace(source, nameof(source));
            Notes = notes;
            FundingRoundId = fundingRoundId;
            TenantId = tenantId;
        }

        internal void UpdateNotes(string notes)
        {
            Notes = notes;
        }
    }
}
