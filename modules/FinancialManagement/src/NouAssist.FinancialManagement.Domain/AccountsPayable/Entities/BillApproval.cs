using System;
using NouAssist.FinancialManagement.AccountsPayable.Enums;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.AccountsPayable.Entities
{
    public class BillApproval : Entity
    {
        public Guid BillId { get; private set; }
        public Guid ApproverId { get; private set; }
        public ApprovalLevel Level { get; private set; }
        public bool IsApproved { get; private set; }
        public DateTime ApprovedDate { get; private set; }
        public string Comments { get; private set; }

        protected BillApproval() { }

        public BillApproval(
            Guid billId,
            Guid approverId,
            ApprovalLevel level,
            bool isApproved,
            string comments = null)
        {
            BillId = billId;
            ApproverId = approverId;
            Level = level;
            IsApproved = isApproved;
            ApprovedDate = DateTime.UtcNow;
            Comments = comments;
        }

        public override object[] GetKeys()
        {
            return new object[] { BillId, ApproverId, Level };
        }
    }
}
