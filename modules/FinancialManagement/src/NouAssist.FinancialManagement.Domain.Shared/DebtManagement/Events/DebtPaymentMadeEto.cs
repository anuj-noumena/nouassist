using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.DebtManagement.Events
{
    [EventName("NouAssist.DebtManagement.DebtPaymentMade")]
    public class DebtPaymentMadeEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid PaymentId { get; set; }
        public Guid DebtAccountId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string DebtAccountName { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal RemainingBalance { get; set; }
        public string Currency { get; set; }

        // For Budget Integration
        public Guid? BudgetCategoryId { get; set; }
        public string PaymentMethod { get; set; }

        // For GL Integration
        public Guid? DebtAccountGLId { get; set; }  // Debt liability account
        public Guid? InterestExpenseGLId { get; set; }  // Interest expense account
        public Guid? PaymentAccountGLId { get; set; }  // Cash/Bank account
    }
}
