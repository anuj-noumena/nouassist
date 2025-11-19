using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Enums;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.AccountsPayable.Services
{
    public class PaymentSchedulingService : DomainService
    {
        public List<PaymentScheduleItem> GeneratePaymentSchedule(
            List<Bill> bills,
            DateTime startDate,
            DateTime endDate)
        {
            var schedule = new List<PaymentScheduleItem>();

            foreach (var bill in bills.Where(b => b.Status == BillStatus.Approved))
            {
                if (bill.DueDate >= startDate && bill.DueDate <= endDate)
                {
                    var priority = CalculatePaymentPriority(bill);
                    schedule.Add(new PaymentScheduleItem(
                        bill.Id,
                        bill.VendorId,
                        bill.DueDate,
                        bill.TotalAmount,
                        priority
                    ));
                }
            }

            return schedule.OrderBy(s => s.DueDate).ThenByDescending(s => s.Priority).ToList();
        }

        public int CalculatePaymentPriority(Bill bill)
        {
            var daysUntilDue = (bill.DueDate - Clock.Now).Days;

            // Higher priority (larger number) for bills due soon
            return daysUntilDue switch
            {
                <= 0 => 100, // Overdue
                <= 3 => 90,  // Due within 3 days
                <= 7 => 80,  // Due within a week
                <= 14 => 70, // Due within 2 weeks
                <= 30 => 60, // Due within a month
                _ => 50      // Due later
            };
        }

        public List<Bill> GetBillsDueForPayment(List<Bill> bills, int daysAhead = 7)
        {
            var cutoffDate = Clock.Now.AddDays(daysAhead);

            return bills
                .Where(b => b.Status == BillStatus.Approved && b.DueDate <= cutoffDate)
                .OrderBy(b => b.DueDate)
                .ToList();
        }

        public decimal CalculateTotalPaymentsForPeriod(List<Bill> bills, DateTime startDate, DateTime endDate)
        {
            return bills
                .Where(b => b.Status == BillStatus.Approved
                    && b.DueDate >= startDate
                    && b.DueDate <= endDate)
                .Sum(b => b.TotalAmount.Amount);
        }

        public CashFlowProjection ProjectCashFlow(List<Bill> bills, int daysAhead = 30)
        {
            var endDate = Clock.Now.AddDays(daysAhead);
            var upcomingPayments = bills
                .Where(b => b.Status == BillStatus.Approved && b.DueDate <= endDate)
                .ToList();

            var totalAmount = upcomingPayments.Sum(b => b.TotalAmount.Amount);
            var paymentsByWeek = upcomingPayments
                .GroupBy(b => GetWeekNumber(b.DueDate))
                .Select(g => new WeeklyPayment(g.Key, g.Sum(b => b.TotalAmount.Amount), g.Count()))
                .ToList();

            return new CashFlowProjection(totalAmount, upcomingPayments.Count, paymentsByWeek);
        }

        private int GetWeekNumber(DateTime date)
        {
            var diff = (date - Clock.Now).Days;
            return (diff / 7) + 1;
        }
    }

    public class PaymentScheduleItem
    {
        public Guid BillId { get; }
        public Guid VendorId { get; }
        public DateTime DueDate { get; }
        public NouAssist.FinancialManagement.Shared.ValueObjects.MonetaryValue Amount { get; }
        public int Priority { get; }

        public PaymentScheduleItem(
            Guid billId,
            Guid vendorId,
            DateTime dueDate,
            NouAssist.FinancialManagement.Shared.ValueObjects.MonetaryValue amount,
            int priority)
        {
            BillId = billId;
            VendorId = vendorId;
            DueDate = dueDate;
            Amount = amount;
            Priority = priority;
        }
    }

    public class CashFlowProjection
    {
        public decimal TotalAmount { get; }
        public int TotalPayments { get; }
        public List<WeeklyPayment> WeeklyBreakdown { get; }

        public CashFlowProjection(decimal totalAmount, int totalPayments, List<WeeklyPayment> weeklyBreakdown)
        {
            TotalAmount = totalAmount;
            TotalPayments = totalPayments;
            WeeklyBreakdown = weeklyBreakdown;
        }
    }

    public class WeeklyPayment
    {
        public int WeekNumber { get; }
        public decimal Amount { get; }
        public int PaymentCount { get; }

        public WeeklyPayment(int weekNumber, decimal amount, int paymentCount)
        {
            WeekNumber = weekNumber;
            Amount = amount;
            PaymentCount = paymentCount;
        }
    }
}
