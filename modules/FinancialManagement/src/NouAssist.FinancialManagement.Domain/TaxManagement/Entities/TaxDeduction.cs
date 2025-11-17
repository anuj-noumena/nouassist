using System;
using NouAssist.TaxManagement.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.TaxManagement.Entities
{
    public class TaxDeduction : Entity
    {
        public Guid TaxReturnId { get; private set; }
        public DeductionCategory Category { get; private set; }
        public string Description { get; private set; }
        public decimal ClaimedAmount { get; private set; }
        public decimal AllowedAmount { get; private set; }
        public Guid? DocumentId { get; private set; }
        public string? Notes { get; set; }
        public bool IsVerified { get; set; }

        protected TaxDeduction() { /* ORM */ }

        public TaxDeduction(
            Guid taxReturnId,
            DeductionCategory category,
            string description,
            decimal claimedAmount,
            decimal allowedAmount,
            Guid? documentId = null)
        {
            TaxReturnId = taxReturnId;
            Category = category;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            ClaimedAmount = Check.Positive(claimedAmount, nameof(claimedAmount));
            AllowedAmount = allowedAmount;
            DocumentId = documentId;
            IsVerified = false;
        }

        public void UpdateAllowedAmount(decimal newAllowedAmount)
        {
            AllowedAmount = newAllowedAmount;
        }

        public void Verify()
        {
            IsVerified = true;
        }

        public override object[] GetKeys() => new object[] { TaxReturnId, Category, Description };
    }
}
