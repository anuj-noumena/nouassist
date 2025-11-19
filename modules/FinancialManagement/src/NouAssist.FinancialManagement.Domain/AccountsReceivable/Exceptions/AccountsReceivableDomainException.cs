using System;
using Volo.Abp;

namespace NouAssist.FinancialManagement.AccountsReceivable.Exceptions
{
    public class AccountsReceivableDomainException : BusinessException
    {
        public AccountsReceivableDomainException(string code, string? message = null, string? details = null, Exception? innerException = null)
            : base(code, message, details, innerException)
        {
        }

        public static AccountsReceivableDomainException InvoiceAlreadyPaid()
        {
            return new AccountsReceivableDomainException(
                "AccountsReceivable:InvoiceAlreadyPaid",
                "This invoice has already been fully paid.");
        }

        public static AccountsReceivableDomainException PaymentExceedsBalance(decimal balance, decimal payment)
        {
            return new AccountsReceivableDomainException(
                "AccountsReceivable:PaymentExceedsBalance",
                $"Payment amount {payment:C} exceeds remaining balance {balance:C}.");
        }

        public static AccountsReceivableDomainException CustomerCreditOnHold()
        {
            return new AccountsReceivableDomainException(
                "AccountsReceivable:CreditOnHold",
                "Customer credit is on hold. Cannot create new invoices.");
        }

        public static AccountsReceivableDomainException CreditLimitExceeded(decimal limit, decimal currentBalance, decimal newAmount)
        {
            return new AccountsReceivableDomainException(
                "AccountsReceivable:CreditLimitExceeded",
                $"Credit limit of {limit:C} would be exceeded. Current: {currentBalance:C}, New charge: {newAmount:C}.");
        }

        public static AccountsReceivableDomainException InvalidInvoiceStatus(string operation, string currentStatus)
        {
            return new AccountsReceivableDomainException(
                "AccountsReceivable:InvalidInvoiceStatus",
                $"Cannot {operation} invoice in status: {currentStatus}.");
        }
    }
}
