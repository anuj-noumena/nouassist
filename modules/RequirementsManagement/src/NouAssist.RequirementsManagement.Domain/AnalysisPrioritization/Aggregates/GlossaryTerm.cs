using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates
{
    /// <summary>
    /// Represents a term in the requirements glossary with its definition
    /// and related information for shared understanding.
    /// </summary>
    public class GlossaryTerm : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Definition { get; private set; }
        public string? Context { get; private set; }
        public string? Examples { get; private set; }
        public string? RelatedTerms { get; private set; } // JSON array of related term IDs
        public string? Synonyms { get; private set; }
        public string? Acronyms { get; private set; }
        public bool IsApproved { get; private set; }
        public Guid? ApprovedBy { get; private set; }
        public DateTime? ApprovedAt { get; private set; }

        protected GlossaryTerm() { }

        public static GlossaryTerm Create(
            Guid id,
            string name,
            string definition,
            Guid? tenantId,
            string? context = null,
            string? examples = null)
        {
            var term = new GlossaryTerm
            {
                Id = id,
                TenantId = tenantId,
                Context = context,
                Examples = examples,
                IsApproved = false
            };

            term.SetName(name);
            term.SetDefinition(definition);

            return term;
        }

        public void UpdateDefinition(string newDefinition)
        {
            SetDefinition(newDefinition);
            
            // Reset approval when definition changes
            if (IsApproved)
            {
                IsApproved = false;
                ApprovedBy = null;
                ApprovedAt = null;
            }
        }

        public void UpdateContext(string? newContext)
        {
            Context = newContext;
        }

        public void UpdateExamples(string? newExamples)
        {
            Examples = newExamples;
        }

        public void SetSynonyms(string? synonyms)
        {
            Synonyms = synonyms;
        }

        public void SetAcronyms(string? acronyms)
        {
            Acronyms = acronyms;
        }

        public void SetRelatedTerms(List<Guid> relatedTermIds)
        {
            RelatedTerms = relatedTermIds.Any() 
                ? System.Text.Json.JsonSerializer.Serialize(relatedTermIds)
                : null;
        }

        public List<Guid> GetRelatedTermIds()
        {
            if (string.IsNullOrEmpty(RelatedTerms))
                return new List<Guid>();

            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(RelatedTerms) ?? new List<Guid>();
            }
            catch
            {
                return new List<Guid>();
            }
        }

        public void Approve(Guid approvedBy, DateTime approvedAt)
        {
            if (IsApproved)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:TermAlreadyApproved");
            }

            IsApproved = true;
            ApprovedBy = approvedBy;
            ApprovedAt = approvedAt;
        }

        public void Revoke()
        {
            if (!IsApproved)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:TermNotApproved");
            }

            IsApproved = false;
            ApprovedBy = null;
            ApprovedAt = null;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: AnalysisConsts.MaxGlossaryTermNameLength
            );
        }

        private void SetDefinition(string definition)
        {
            Definition = Check.NotNullOrWhiteSpace(
                definition,
                nameof(definition),
                maxLength: AnalysisConsts.MaxGlossaryTermDefinitionLength
            );
        }

        public override string ToString()
        {
            return $"{Name}: {Definition}";
        }
    }
}
