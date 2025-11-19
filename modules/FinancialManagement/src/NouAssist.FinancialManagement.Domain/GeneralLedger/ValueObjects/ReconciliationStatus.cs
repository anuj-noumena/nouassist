using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.GeneralLedger.ValueObjects
{
    public class ReconciliationStatus : ValueObject
    {
        public bool IsReconciled { get; private set; }
        public DateTime? ReconciledDate { get; private set; }
        public Guid? ReconciledBy { get; private set; }
        public string ReconciledWith { get; private set; }

        private ReconciliationStatus() { }

        private ReconciliationStatus(bool isReconciled, DateTime? reconciledDate, Guid? reconciledBy, string reconciledWith)
        {
            IsReconciled = isReconciled;
            ReconciledDate = reconciledDate;
            ReconciledBy = reconciledBy;
            ReconciledWith = reconciledWith;
        }

        public static ReconciliationStatus NotReconciled() => new ReconciliationStatus(false, null, null, null);

        public static ReconciliationStatus Reconciled(DateTime date, Guid userId, string reconciledWith)
            => new ReconciliationStatus(true, date, userId, reconciledWith);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return IsReconciled;
            yield return ReconciledDate ?? DateTime.MinValue;
            yield return ReconciledBy ?? Guid.Empty;
            yield return ReconciledWith ?? string.Empty;
        }
    }
}
