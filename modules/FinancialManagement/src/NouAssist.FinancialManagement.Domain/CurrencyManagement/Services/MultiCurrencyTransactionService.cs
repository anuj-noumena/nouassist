using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.CurrencyManagement.Services
{
    /// <summary>
    /// Domain service for handling multi-currency transactions in AP/AR
    /// Provides currency conversion and FX gain/loss tracking
    /// </summary>
    public class MultiCurrencyTransactionService : DomainService
    {
        private readonly ILogger<MultiCurrencyTransactionService> _logger;

        public MultiCurrencyTransactionService(
            ILogger<MultiCurrencyTransactionService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Convert foreign currency bill to base currency
        /// Stores both original and converted amounts for reporting
        /// </summary>
        public async Task<CurrencyConversionResult> ConvertBillToBaseCurrencyAsync(
            MonetaryValue billAmount,
            string baseCurrency,
            DateTime transactionDate,
            Guid? tenantId = null)
        {
            if (billAmount.Currency == baseCurrency)
            {
                // No conversion needed
                return new CurrencyConversionResult
                {
                    OriginalAmount = billAmount,
                    ConvertedAmount = billAmount,
                    ExchangeRate = 1.0m,
                    ConversionDate = transactionDate,
                    BaseCurrency = baseCurrency,
                    ForeignCurrency = billAmount.Currency
                };
            }

            _logger.LogInformation(
                "Converting bill amount: {Amount} {Currency} to {BaseCurrency}",
                billAmount.Amount,
                billAmount.Currency,
                baseCurrency);

            // Fetch exchange rate
            var exchangeRate = await GetExchangeRateAsync(
                billAmount.Currency,
                baseCurrency,
                transactionDate,
                tenantId);

            var convertedAmount = new MonetaryValue(
                billAmount.Amount * exchangeRate,
                baseCurrency);

            return new CurrencyConversionResult
            {
                OriginalAmount = billAmount,
                ConvertedAmount = convertedAmount,
                ExchangeRate = exchangeRate,
                ConversionDate = transactionDate,
                BaseCurrency = baseCurrency,
                ForeignCurrency = billAmount.Currency,
                ConversionNote = $"Converted at rate {exchangeRate:F4} on {transactionDate:yyyy-MM-dd}"
            };
        }

        /// <summary>
        /// Calculate FX gain/loss when paying foreign currency bill
        /// Compares exchange rate at bill date vs payment date
        /// </summary>
        public FxGainLossResult CalculateFxGainLoss(
            MonetaryValue originalBillAmount,
            decimal originalExchangeRate,
            DateTime billDate,
            decimal paymentExchangeRate,
            DateTime paymentDate,
            string baseCurrency)
        {
            var originalBaseAmount = originalBillAmount.Amount * originalExchangeRate;
            var paymentBaseAmount = originalBillAmount.Amount * paymentExchangeRate;
            var gainLoss = originalBaseAmount - paymentBaseAmount;

            var result = new FxGainLossResult
            {
                ForeignCurrency = originalBillAmount.Currency,
                BaseCurrency = baseCurrency,
                ForeignAmount = originalBillAmount.Amount,
                OriginalRate = originalExchangeRate,
                PaymentRate = paymentExchangeRate,
                OriginalBaseAmount = originalBaseAmount,
                PaymentBaseAmount = paymentBaseAmount,
                GainLoss = gainLoss,
                IsGain = gainLoss > 0,
                BillDate = billDate,
                PaymentDate = paymentDate
            };

            if (Math.Abs(gainLoss) > 0.01m)
            {
                _logger.LogInformation(
                    "FX {GainOrLoss}: {Amount} {Currency} - Rate changed from {OriginalRate:F4} to {PaymentRate:F4}",
                    gainLoss > 0 ? "Gain" : "Loss",
                    Math.Abs(gainLoss),
                    baseCurrency,
                    originalExchangeRate,
                    paymentExchangeRate);
            }

            return result;
        }

        /// <summary>
        /// Revalue open foreign currency invoices at month-end
        /// Required for accurate financial reporting
        /// </summary>
        public async Task<RevaluationResult> RevalueOpenInvoicesAsync(
            MonetaryValue[] openInvoiceAmounts,
            decimal[] originalRates,
            string baseCurrency,
            DateTime revaluationDate,
            Guid? tenantId = null)
        {
            _logger.LogInformation(
                "Revaluing {Count} open invoices as of {Date}",
                openInvoiceAmounts.Length,
                revaluationDate);

            var totalOriginalBase = 0m;
            var totalRevaluedBase = 0m;

            for (int i = 0; i < openInvoiceAmounts.Length; i++)
            {
                var invoice = openInvoiceAmounts[i];
                var originalRate = originalRates[i];

                if (invoice.Currency == baseCurrency)
                    continue;

                // Get current exchange rate
                var currentRate = await GetExchangeRateAsync(
                    invoice.Currency,
                    baseCurrency,
                    revaluationDate,
                    tenantId);

                totalOriginalBase += invoice.Amount * originalRate;
                totalRevaluedBase += invoice.Amount * currentRate;
            }

            var revaluationGainLoss = totalRevaluedBase - totalOriginalBase;

            return new RevaluationResult
            {
                RevaluationDate = revaluationDate,
                InvoiceCount = openInvoiceAmounts.Length,
                OriginalBaseValue = totalOriginalBase,
                RevaluedBaseValue = totalRevaluedBase,
                GainLoss = revaluationGainLoss,
                IsGain = revaluationGainLoss > 0,
                BaseCurrency = baseCurrency
            };
        }

        /// <summary>
        /// Get exchange rate for a specific date
        /// In production, query CurrencyExchangeRate repository
        /// </summary>
        private async Task<decimal> GetExchangeRateAsync(
            string fromCurrency,
            string toCurrency,
            DateTime date,
            Guid? tenantId)
        {
            // TODO: Query CurrencyExchangeRate repository
            // For now, return placeholder rates

            _logger.LogWarning(
                "Using placeholder exchange rate for {From} -> {To}. Implement repository query in production.",
                fromCurrency,
                toCurrency);

            // Placeholder rates (as of sample date)
            var rates = new System.Collections.Generic.Dictionary<string, decimal>
            {
                ["EUR-USD"] = 1.08m,
                ["GBP-USD"] = 1.27m,
                ["JPY-USD"] = 0.0067m,
                ["CAD-USD"] = 0.74m,
                ["AUD-USD"] = 0.66m,
                ["USD-EUR"] = 0.93m,
                ["USD-GBP"] = 0.79m,
                ["USD-JPY"] = 149.5m
            };

            var key = $"{fromCurrency}-{toCurrency}";
            if (rates.ContainsKey(key))
            {
                return await Task.FromResult(rates[key]);
            }

            // If reverse rate exists
            var reverseKey = $"{toCurrency}-{fromCurrency}";
            if (rates.ContainsKey(reverseKey))
            {
                return await Task.FromResult(1 / rates[reverseKey]);
            }

            throw new BusinessException(
                "Currency:ExchangeRateNotFound",
                $"Exchange rate not found for {fromCurrency} to {toCurrency} on {date:yyyy-MM-dd}");
        }
    }

    public class CurrencyConversionResult
    {
        public MonetaryValue OriginalAmount { get; set; }
        public MonetaryValue ConvertedAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime ConversionDate { get; set; }
        public string BaseCurrency { get; set; }
        public string ForeignCurrency { get; set; }
        public string ConversionNote { get; set; }
    }

    public class FxGainLossResult
    {
        public string ForeignCurrency { get; set; }
        public string BaseCurrency { get; set; }
        public decimal ForeignAmount { get; set; }
        public decimal OriginalRate { get; set; }
        public decimal PaymentRate { get; set; }
        public decimal OriginalBaseAmount { get; set; }
        public decimal PaymentBaseAmount { get; set; }
        public decimal GainLoss { get; set; }
        public bool IsGain { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime PaymentDate { get; set; }
    }

    public class RevaluationResult
    {
        public DateTime RevaluationDate { get; set; }
        public int InvoiceCount { get; set; }
        public decimal OriginalBaseValue { get; set; }
        public decimal RevaluedBaseValue { get; set; }
        public decimal GainLoss { get; set; }
        public bool IsGain { get; set; }
        public string BaseCurrency { get; set; }
    }
}
