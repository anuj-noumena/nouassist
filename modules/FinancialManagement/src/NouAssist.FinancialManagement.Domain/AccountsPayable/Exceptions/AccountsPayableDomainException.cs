using System;
using Volo.Abp;

namespace NouAssist.FinancialManagement.AccountsPayable.Exceptions
{
    public class AccountsPayableDomainException : BusinessException
    {
        public AccountsPayableDomainException(string code, string? message = null, string? details = null, Exception? innerException = null)
            : base(code, message, details, innerException)
        {
        }

        public static AccountsPayableDomainException BillAlreadyPaid()
        {
            return new AccountsPayableDomainException(
                "AccountsPayable:BillAlreadyPaid",
                "This bill has already been paid.");
        }

        public static AccountsPayableDomainException BillNotApproved()
        {
            return new AccountsPayableDomainException(
                "AccountsPayable:BillNotApproved",
                "Bill must be approved before payment.");
        }

        public static AccountsPayableDomainException InvalidApprovalLevel(int requiredLevel, int currentLevel)
        {
            return new AccountsPayableDomainException(
                "AccountsPayable:InvalidApprovalLevel",
                $"Approval level {currentLevel} is insufficient. Required level: {requiredLevel}.");
        }

        public static AccountsPayableDomainException VendorCreditLimitExceeded(decimal limit, decimal attempted)
        {
            return new AccountsPayableDomainException(
                "AccountsPayable:CreditLimitExceeded",
                $"Vendor credit limit of {limit:C} would be exceeded. Attempted: {attempted:C}.");
        }
    }
}
