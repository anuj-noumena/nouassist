using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Entities
{
    /// <summary>
    /// Records all transactions affecting the cap table
    /// </summary>
    public class TransactionRecord : FullAuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public Guid CapTableId { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public Guid? FromHolderId { get; private set; }
        public Guid? ToHolderId { get; private set; }
        public Guid? SecurityIssuanceId { get; private set; }
        public Guid? ShareClassId { get; private set; }
        public ShareQuantity Quantity { get; private set; }
        public PricePerShare PricePerShare { get; private set; }
        public InvestmentAmount TotalAmount { get; private set; }
        public string Memo { get; private set; }
        public string ExternalReference { get; private set; }

        protected TransactionRecord() { }

        internal TransactionRecord(
            Guid id,
            Guid capTableId,
            TransactionType transactionType,
            DateTime transactionDate,
            ShareQuantity quantity,
            PricePerShare pricePerShare,
            string memo = null,
            Guid? fromHolderId = null,
            Guid? toHolderId = null,
            Guid? securityIssuanceId = null,
            Guid? shareClassId = null,
            string externalReference = null,
            Guid? tenantId = null) : base(id)
        {
            CapTableId = capTableId;
            TransactionType = transactionType;
            TransactionDate = transactionDate;
            SetQuantity(quantity);
            SetPricePerShare(pricePerShare);
            TotalAmount = pricePerShare.CalculateInvestment(quantity);
            FromHolderId = fromHolderId;
            ToHolderId = toHolderId;
            SecurityIssuanceId = securityIssuanceId;
            ShareClassId = shareClassId;
            SetMemo(memo);
            ExternalReference = externalReference;
            TenantId = tenantId;
        }

        private void SetQuantity(ShareQuantity quantity)
        {
            Quantity = quantity ?? throw new ArgumentNullException(nameof(quantity));
        }

        private void SetPricePerShare(PricePerShare pricePerShare)
        {
            PricePerShare = pricePerShare ?? throw new ArgumentNullException(nameof(pricePerShare));
        }

        private void SetMemo(string memo)
        {
            Memo = Check.Length(
                memo,
                nameof(memo),
                maxLength: CapTableConsts.MaxTransactionMemoLength
            );
        }

        internal void UpdateMemo(string memo)
        {
            SetMemo(memo);
        }
    }
}
