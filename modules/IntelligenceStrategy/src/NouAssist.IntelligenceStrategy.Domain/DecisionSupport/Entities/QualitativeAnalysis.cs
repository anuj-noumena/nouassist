using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Entities
{
    /// <summary>
    /// Represents a qualitative analysis (SWOT, Risk) for a decision model
    /// </summary>
    public class QualitativeAnalysis : Entity<Guid>
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
        /// The type of analysis (SWOT, Risk)
        /// </summary>
        public AnalysisType AnalysisType { get; private set; }
        
        /// <summary>
        /// SWOT items if this is a SWOT analysis
        /// </summary>
        private readonly List<SwotItem> _swotItems;
        public IReadOnlyCollection<SwotItem> SwotItems => _swotItems?.AsReadOnly();
        
        /// <summary>
        /// Risk items if this is a Risk analysis
        /// </summary>
        private readonly List<RiskItem> _riskItems;
        public IReadOnlyCollection<RiskItem> RiskItems => _riskItems?.AsReadOnly();

        protected QualitativeAnalysis()
        {
            _swotItems = new List<SwotItem>();
            _riskItems = new List<RiskItem>();
        }

        /// <summary>
        /// Creates a new qualitative analysis
        /// </summary>
        public QualitativeAnalysis(
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
            
            if (analysisType == AnalysisType.Swot)
            {
                _swotItems = new List<SwotItem>();
                _riskItems = null;
            }
            else if (analysisType == AnalysisType.Risk)
            {
                _swotItems = null;
                _riskItems = new List<RiskItem>();
            }
        }
        
        /// <summary>
        /// Adds a SWOT item to the analysis
        /// </summary>
        public void AddSwotItem(SwotItem item)
        {
            if (AnalysisType != AnalysisType.Swot)
            {
                throw new InvalidOperationException("Cannot add SWOT items to a non-SWOT analysis");
            }
            
            _swotItems.Add(item);
        }
        
        /// <summary>
        /// Adds a risk item to the analysis
        /// </summary>
        public void AddRiskItem(string description, Impact impact, Probability probability, string mitigationStrategy = null)
        {
            if (AnalysisType != AnalysisType.Risk)
            {
                throw new InvalidOperationException("Cannot add risk items to a non-Risk analysis");
            }
            
            _riskItems.Add(new RiskItem(description, impact, probability, mitigationStrategy));
        }
        
        /// <summary>
        /// Updates the analysis properties
        /// </summary>
        public void Update(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
    
    /// <summary>
    /// Represents a risk item in a risk analysis
    /// </summary>
    public class RiskItem : ValueObject
    {
        /// <summary>
        /// Description of the risk
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// The impact if the risk occurs
        /// </summary>
        public Impact Impact { get; private set; }
        
        /// <summary>
        /// The probability of the risk occurring
        /// </summary>
        public Probability Probability { get; private set; }
        
        /// <summary>
        /// Strategy to mitigate the risk
        /// </summary>
        public string MitigationStrategy { get; private set; }
        
        /// <summary>
        /// Risk score (Impact * Probability)
        /// </summary>
        public decimal RiskScore => Impact.Value * Probability.Value;

        protected RiskItem()
        {
        }

        /// <summary>
        /// Creates a new risk item
        /// </summary>
        public RiskItem(
            string description,
            Impact impact,
            Probability probability,
            string mitigationStrategy = null)
        {
            Description = description;
            Impact = impact;
            Probability = probability;
            MitigationStrategy = mitigationStrategy;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Description;
            yield return Impact;
            yield return Probability;
            yield return MitigationStrategy;
        }
    }
}
