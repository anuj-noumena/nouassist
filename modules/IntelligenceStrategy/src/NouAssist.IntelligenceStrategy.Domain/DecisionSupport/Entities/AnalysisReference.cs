using System;
using NouAssist.IntelligenceStrategy.DecisionSupport.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Entities
{
    /// <summary>
    /// Represents a reference to an analysis that supports a strategic initiative
    /// </summary>
    public class AnalysisReference : Entity
    {
        /// <summary>
        /// The strategic initiative that this analysis supports
        /// </summary>
        public Guid StrategicInitiativeId { get; private set; }
        
        /// <summary>
        /// Type of analysis (e.g., "SWOT", "ROI", "RiskAssessment")
        /// </summary>
        public string ReferenceType { get; private set; }
        
        /// <summary>
        /// Key or ID to locate the reference
        /// </summary>
        public string ReferenceKey { get; private set; }
        
        /// <summary>
        /// Description of the analysis and how it supports the initiative
        /// </summary>
        public string Description { get; private set; }

        protected AnalysisReference() { }

        /// <summary>
        /// Creates a new instance of <see cref="AnalysisReference"/>
        /// </summary>
        internal AnalysisReference(
            Guid strategicInitiativeId,
            string referenceType,
            string referenceKey,
            string description)
        {
            StrategicInitiativeId = strategicInitiativeId;
            SetReferenceType(referenceType);
            SetReferenceKey(referenceKey);
            SetDescription(description);
        }

        private void SetReferenceType(string referenceType)
        {
            ReferenceType = Check.NotNullOrWhiteSpace(
                referenceType,
                nameof(referenceType),
                maxLength: 50
            );
        }

        private void SetReferenceKey(string referenceKey)
        {
            ReferenceKey = Check.NotNullOrWhiteSpace(
                referenceKey,
                nameof(referenceKey),
                maxLength: DecisionSupportConsts.MaxReferenceKeyLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: DecisionSupportConsts.MaxDescriptionLength
            );
        }

        /// <inheritdoc/>
        public override object[] GetKeys()
        {
            return new object[] { StrategicInitiativeId, ReferenceType, ReferenceKey };
        }
    }
}
