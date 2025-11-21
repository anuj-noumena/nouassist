using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects
{
    /// <summary>
    /// Value object for term sheet comparison data
    /// </summary>
    public class TermComparisonData : ValueObject
    {
        public decimal PreMoneyValuation { get; private set; }
        public decimal PostMoneyValuation { get; private set; }
        public decimal InvestmentAmount { get; private set; }
        public decimal EquityPercentage { get; private set; }
        public bool HasLiquidationPreference { get; private set; }
        public decimal LiquidationMultiple { get; private set; }
        public bool HasParticipationRights { get; private set; }
        public bool HasProRataRights { get; private set; }
        public bool HasAntiDilutionProtection { get; private set; }
        public Dictionary<string, bool> InvestorRights { get; private set; }
        
        private TermComparisonData() 
        {
            InvestorRights = new Dictionary<string, bool>();
        }
        
        public TermComparisonData(
            decimal preMoneyValuation,
            decimal investmentAmount,
            bool hasLiquidationPreference,
            decimal liquidationMultiple,
            bool hasParticipationRights,
            bool hasProRataRights,
            bool hasAntiDilutionProtection,
            Dictionary<string, bool> investorRights)
        {
            if (preMoneyValuation <= 0)
            {
                throw new ArgumentException("Pre-money valuation must be greater than zero.");
            }
            
            if (investmentAmount <= 0)
            {
                throw new ArgumentException("Investment amount must be greater than zero.");
            }
            
            if (liquidationMultiple < 1)
            {
                throw new ArgumentException("Liquidation multiple must be at least 1.");
            }
            
            PreMoneyValuation = preMoneyValuation;
            InvestmentAmount = investmentAmount;
            PostMoneyValuation = preMoneyValuation + investmentAmount;
            EquityPercentage = investmentAmount / PostMoneyValuation * 100;
            HasLiquidationPreference = hasLiquidationPreference;
            LiquidationMultiple = liquidationMultiple;
            HasParticipationRights = hasParticipationRights;
            HasProRataRights = hasProRataRights;
            HasAntiDilutionProtection = hasAntiDilutionProtection;
            InvestorRights = investorRights ?? new Dictionary<string, bool>();
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PreMoneyValuation;
            yield return PostMoneyValuation;
            yield return InvestmentAmount;
            yield return EquityPercentage;
            yield return HasLiquidationPreference;
            yield return LiquidationMultiple;
            yield return HasParticipationRights;
            yield return HasProRataRights;
            yield return HasAntiDilutionProtection;
            
            // Sort dictionary keys for deterministic comparison
            foreach (var key in InvestorRights.Keys.OrderBy(k => k))
            {
                yield return key;
                yield return InvestorRights[key];
            }
        }
    }
}
