using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.AccountsReceivable.Services
{
    public class InvoiceAgingService : DomainService
    {
        public AgingReport GenerateAgingReport(List<Invoice> invoices, DateTime? asOfDate = null)
        {
            var reportDate = asOfDate ?? Clock.Now;
            var outstandingInvoices = invoices
                .Where(i => i.Status == InvoiceStatus.Sent || i.Status == InvoiceStatus.Overdue)
                .ToList();

            var agingBuckets = new AgingBuckets();

            foreach (var invoice in outstandingInvoices)
            {
                var daysOutstanding = CalculateDaysOutstanding(invoice, reportDate);
                var amount = invoice.TotalAmount.Amount;

                if (daysOutstanding <= 30)
                {
                    agingBuckets.Current += amount;
                }
                else if (daysOutstanding <= 60)
                {
                    agingBuckets.Days31To60 += amount;
                }
                else if (daysOutstanding <= 90)
                {
                    agingBuckets.Days61To90 += amount;
                }
                else
                {
                    agingBuckets.Over90Days += amount;
                }
            }

            return new AgingReport(reportDate, agingBuckets, outstandingInvoices.Count);
        }

        public int CalculateDaysOutstanding(Invoice invoice, DateTime? asOfDate = null)
        {
            var date = asOfDate ?? Clock.Now;
            return Math.Max(0, (date - invoice.DueDate).Days);
        }

        public AgingCategory DetermineAgingCategory(Invoice invoice, DateTime? asOfDate = null)
        {
            var daysOutstanding = CalculateDaysOutstanding(invoice, asOfDate);

            return daysOutstanding switch
            {
                <= 30 => AgingCategory.Current,
                <= 60 => AgingCategory.Days31To60,
                <= 90 => AgingCategory.Days61To90,
                _ => AgingCategory.Over90Days
            };
        }

        public Dictionary<AgingCategory, List<Invoice>> GroupInvoicesByAging(
            List<Invoice> invoices,
            DateTime? asOfDate = null)
        {
            return invoices
                .Where(i => i.Status == InvoiceStatus.Sent || i.Status == InvoiceStatus.Overdue)
                .GroupBy(i => DetermineAgingCategory(i, asOfDate))
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public List<Invoice> GetOverdueInvoices(List<Invoice> invoices, int minimumDaysOverdue = 0)
        {
            return invoices
                .Where(i => (i.Status == InvoiceStatus.Sent || i.Status == InvoiceStatus.Overdue)
                    && CalculateDaysOutstanding(i) > minimumDaysOverdue)
                .OrderByDescending(i => CalculateDaysOutstanding(i))
                .ToList();
        }

        public CustomerAgingSummary GenerateCustomerAgingSummary(
            Customer customer,
            List<Invoice> customerInvoices)
        {
            var outstandingInvoices = customerInvoices
                .Where(i => i.Status == InvoiceStatus.Sent || i.Status == InvoiceStatus.Overdue)
                .ToList();

            var totalOutstanding = outstandingInvoices.Sum(i => i.TotalAmount.Amount);
            var oldestInvoice = outstandingInvoices.MinBy(i => i.DueDate);
            var averageDaysOutstanding = outstandingInvoices.Any()
                ? (int)outstandingInvoices.Average(i => CalculateDaysOutstanding(i))
                : 0;

            return new CustomerAgingSummary(
                customer.Id,
                customer.CustomerName,
                totalOutstanding,
                outstandingInvoices.Count,
                averageDaysOutstanding,
                oldestInvoice?.DueDate
            );
        }
    }

    public class AgingReport
    {
        public DateTime ReportDate { get; }
        public AgingBuckets Buckets { get; }
        public int TotalInvoices { get; }
        public decimal TotalAmount => Buckets.TotalAmount;

        public AgingReport(DateTime reportDate, AgingBuckets buckets, int totalInvoices)
        {
            ReportDate = reportDate;
            Buckets = buckets;
            TotalInvoices = totalInvoices;
        }
    }

    public class AgingBuckets
    {
        public decimal Current { get; set; }
        public decimal Days31To60 { get; set; }
        public decimal Days61To90 { get; set; }
        public decimal Over90Days { get; set; }
        public decimal TotalAmount => Current + Days31To60 + Days61To90 + Over90Days;
    }

    public enum AgingCategory
    {
        Current = 1,
        Days31To60 = 2,
        Days61To90 = 3,
        Over90Days = 4
    }

    public class CustomerAgingSummary
    {
        public Guid CustomerId { get; }
        public string CustomerName { get; }
        public decimal TotalOutstanding { get; }
        public int InvoiceCount { get; }
        public int AverageDaysOutstanding { get; }
        public DateTime? OldestInvoiceDate { get; }

        public CustomerAgingSummary(
            Guid customerId,
            string customerName,
            decimal totalOutstanding,
            int invoiceCount,
            int averageDaysOutstanding,
            DateTime? oldestInvoiceDate)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            TotalOutstanding = totalOutstanding;
            InvoiceCount = invoiceCount;
            AverageDaysOutstanding = averageDaysOutstanding;
            OldestInvoiceDate = oldestInvoiceDate;
        }
    }
}
