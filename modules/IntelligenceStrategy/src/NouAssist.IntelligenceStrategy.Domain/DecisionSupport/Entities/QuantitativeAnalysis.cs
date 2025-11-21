using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Entities
{
    /// <summary>
    /// Represents a quantitative analysis (Cost-Benefit, ROI) for a decision model
    /// </summary>
    public class QuantitativeAnalysis : Entity<Guid>
    {
        /// <summary>
        /// The ID of the decision model this analysis belongs to
        /// </summary>
        public Guid DecisionModelId { get; private set; }
        
        /// <summary>
        /// The title of the analysis
        /// </summary>
        public string Title { get; private set; }
        
        /// <summary>
        /// The description of the analysis
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// The type of analysis (Cost-Benefit, ROI)
        /// </summary>
        public AnalysisType AnalysisType { get; private set; }
        
        /// <summary>
        /// Cost items if this is a Cost-Benefit analysis
        /// </summary>
        private readonly List<CostValue> _costItems;
        public IReadOnlyCollection<CostValue> CostItems => _costItems?.AsReadOnly();
        
        /// <summary>
        /// Benefit items if this is a Cost-Benefit analysis
        /// </summary>
        private readonly List<BenefitValue> _benefitItems;
        public IReadOnlyCollection<BenefitValue> BenefitItems => _benefitItems?.AsReadOnly();
        
        /// <summary>
        /// ROI value if this is an ROI analysis
        /// </summary>
        public RoiValue RoiValue { get; private set; }

        protected QuantitativeAnalysis()
        {
            _costItems = new List<CostValue>();
            _benefitItems = new List<BenefitValue>();
        }

        /// <summary>
        /// Creates a new quantitative analysis
        /// </summary>
        public QuantitativeAnalysis(
            Guid id,
            Guid decisionModelId,
            string title,
            string description,
            AnalysisType analysisType)
            : base(id)
        {
            DecisionModelId = decisionModelId;
            Title = title;
            Description = description;
            AnalysisType = analysisType;
            
            if (analysisType == AnalysisType.CostBenefit)
            {
                _costItems = new List<CostValue>();
                _benefitItems = new List<BenefitValue>();
            }
        }
        
        /// <summary>
        /// Adds a cost item to the analysis
        /// </summary>
        public void AddCostItem(CostValue costValue)
        {
            if (AnalysisType != AnalysisType.CostBenefit)
            {
                throw new InvalidOperationException("Cannot add cost items to a non-Cost-Benefit analysis");
            }
            
            _costItems.Add(costValue);
        }
        
        /// <summary>
        /// Adds a benefit item to the analysis
        /// </summary>
        public void AddBenefitItem(BenefitValue benefitValue)
        {
            if (AnalysisType != AnalysisType.CostBenefit)
            {
                throw new InvalidOperationException("Cannot add benefit items to a non-Cost-Benefit analysis");
            }
            
            _benefitItems.Add(benefitValue);
        }
        
        /// <summary>
        /// Sets the ROI value for the analysis
        /// </summary>
        public void SetRoiValue(decimal netBenefit, decimal totalCost, int timeframeMonths)
        {
            if (AnalysisType != AnalysisType.Roi)
            {
                throw new InvalidOperationException("Cannot set ROI value for a non-ROI analysis");
            }
            
            RoiValue = new RoiValue(netBenefit, totalCost, timeframeMonths);
        }
        
        /// <summary>
        /// Updates the analysis properties
        /// </summary>
        public void Update(string title, string description)
        {
            Title = title;
            Description = description;
        }
        
        /// <summary>
        /// Calculates the total cost for a Cost-Benefit analysis
        /// </summary>
        public decimal CalculateTotalCost()
        {
            if (AnalysisType != AnalysisType.CostBenefit)
            {
                throw new InvalidOperationException("Cannot calculate total cost for a non-Cost-Benefit analysis");
            }
            
            decimal total = 0;
            
            foreach (var cost in _costItems)
            {
                total += cost.Value;
            }
            
            return total;
        }
        
        /// <summary>
        /// Calculates the total monetary benefit for a Cost-Benefit analysis
        /// </summary>
        public decimal CalculateTotalMonetaryBenefit()
        {
            if (AnalysisType != AnalysisType.CostBenefit)
            {
                throw new InvalidOperationException("Cannot calculate total benefit for a non-Cost-Benefit analysis");
            }
            
            decimal total = 0;
            
            foreach (var benefit in _benefitItems)
            {
                if (benefit.MonetaryValue.HasValue)
                {
                    total += benefit.MonetaryValue.Value;
                }
            }
            
            return total;
        }
    }
}
