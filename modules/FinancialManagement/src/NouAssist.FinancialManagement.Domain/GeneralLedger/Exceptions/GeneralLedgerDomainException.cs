using System;
using Volo.Abp;

namespace NouAssist.FinancialManagement.GeneralLedger.Exceptions
{
    public class GeneralLedgerDomainException : BusinessException
    {
        public GeneralLedgerDomainException(string code, string? message = null, string? details = null, Exception? innerException = null)
            : base(code, message, details, innerException)
        {
        }

        public static GeneralLedgerDomainException JournalEntryNotBalanced(decimal debitTotal, decimal creditTotal)
        {
            return new GeneralLedgerDomainException(
                "GeneralLedger:JournalNotBalanced",
                $"Journal entry is not balanced. Debits: {debitTotal:C}, Credits: {creditTotal:C}.");
        }

        public static GeneralLedgerDomainException AccountingPeriodClosed()
        {
            return new GeneralLedgerDomainException(
                "GeneralLedger:PeriodClosed",
                "Cannot post entries to a closed accounting period.");
        }

        public static GeneralLedgerDomainException InvalidAccountType(string expectedType, string actualType)
        {
            return new GeneralLedgerDomainException(
                "GeneralLedger:InvalidAccountType",
                $"Expected account type {expectedType}, but found {actualType}.");
        }

        public static GeneralLedgerDomainException AccountBalanceMismatch(string accountNumber, decimal expected, decimal actual)
        {
            return new GeneralLedgerDomainException(
                "GeneralLedger:BalanceMismatch",
                $"Account {accountNumber} balance mismatch. Expected: {expected:C}, Actual: {actual:C}.");
        }

        public static GeneralLedgerDomainException CannotDeletePostedEntry()
        {
            return new GeneralLedgerDomainException(
                "GeneralLedger:CannotDeletePosted",
                "Cannot delete a posted journal entry. Reverse it instead.");
        }
    }
}
