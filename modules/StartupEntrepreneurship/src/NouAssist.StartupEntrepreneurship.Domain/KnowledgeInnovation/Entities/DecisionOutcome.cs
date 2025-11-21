using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities
{
    /// <summary>
    /// Represents the outcome of an implemented decision
    /// </summary>
    public class DecisionOutcome : Entity
    {
        public Guid DecisionRecordId { get; private set; }
        public string Description { get; private set; }
        public DateTime? ImplementationDate { get; private set; }
        public string LessonsLearned { get; private set; }
        public List<OutcomeImpact> Impacts { get; private set; }

        protected DecisionOutcome()
        {
            Impacts = new List<OutcomeImpact>();
        }

        internal DecisionOutcome(
            Guid decisionRecordId, 
            string description, 
            DateTime? implementationDate = null, 
            string lessonsLearned = null)
        {
            DecisionRecordId = decisionRecordId;
            
            Description = Check.NotNullOrWhiteSpace(
                description, 
                nameof(description), 
                maxLength: KnowledgeInnovationConsts.DecisionRecord.OutcomeMaxLength);
            
            ImplementationDate = implementationDate;
            LessonsLearned = lessonsLearned;
            Impacts = new List<OutcomeImpact>();
        }

        internal void Update(string description, DateTime? implementationDate, string lessonsLearned)
        {
            Description = Check.NotNullOrWhiteSpace(
                description, 
                nameof(description), 
                maxLength: KnowledgeInnovationConsts.DecisionRecord.OutcomeMaxLength);
            
            ImplementationDate = implementationDate;
            LessonsLearned = lessonsLearned;
        }

        internal void AddImpact(OutcomeImpact impact)
        {
            Impacts.Add(impact);
        }

        internal void RemoveImpact(string area)
        {
            var impactToRemove = Impacts.FirstOrDefault(i => i.Area == area);
            if (impactToRemove != null)
            {
                Impacts.Remove(impactToRemove);
            }
        }

        internal void ClearImpacts()
        {
            Impacts.Clear();
        }

        public override object[] GetKeys()
        {
            return new object[] { DecisionRecordId };
        }
    }
}
