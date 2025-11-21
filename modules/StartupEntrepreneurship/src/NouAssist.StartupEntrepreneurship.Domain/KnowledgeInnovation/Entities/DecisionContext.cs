using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities
{
    /// <summary>
    /// Represents the context in which a decision was made
    /// </summary>
    public class DecisionContext : Entity
    {
        public Guid DecisionRecordId { get; private set; }
        public string Background { get; private set; }
        public string ProblemStatement { get; private set; }
        public List<string> Stakeholders { get; private set; }
        public List<string> Constraints { get; private set; }
        public DateTime ContextCapturedAt { get; private set; }

        protected DecisionContext()
        {
            Stakeholders = new List<string>();
            Constraints = new List<string>();
        }

        internal DecisionContext(
            Guid decisionRecordId, 
            string background, 
            string problemStatement, 
            List<string> stakeholders = null, 
            List<string> constraints = null, 
            DateTime? contextCapturedAt = null)
        {
            DecisionRecordId = decisionRecordId;
            
            Background = Check.NotNullOrWhiteSpace(
                background, 
                nameof(background), 
                maxLength: KnowledgeInnovationConsts.DecisionRecord.ContextMaxLength);
            
            ProblemStatement = Check.NotNullOrWhiteSpace(
                problemStatement, 
                nameof(problemStatement), 
                maxLength: KnowledgeInnovationConsts.DecisionRecord.ContextMaxLength);
            
            Stakeholders = stakeholders ?? new List<string>();
            Constraints = constraints ?? new List<string>();
            ContextCapturedAt = contextCapturedAt ?? DateTime.UtcNow;
        }

        internal void Update(
            string background, 
            string problemStatement, 
            List<string> stakeholders, 
            List<string> constraints)
        {
            Background = Check.NotNullOrWhiteSpace(
                background, 
                nameof(background), 
                maxLength: KnowledgeInnovationConsts.DecisionRecord.ContextMaxLength);
            
            ProblemStatement = Check.NotNullOrWhiteSpace(
                problemStatement, 
                nameof(problemStatement), 
                maxLength: KnowledgeInnovationConsts.DecisionRecord.ContextMaxLength);
            
            Stakeholders = stakeholders ?? new List<string>();
            Constraints = constraints ?? new List<string>();
        }

        public override object[] GetKeys()
        {
            return new object[] { DecisionRecordId };
        }
    }
}
