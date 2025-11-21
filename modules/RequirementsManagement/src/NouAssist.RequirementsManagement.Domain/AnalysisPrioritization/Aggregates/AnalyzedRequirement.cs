using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Entities;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Events;
using NouAssist.RequirementsManagement.AnalysisPrioritization.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates
{
    /// <summary>
    /// Represents an analyzed requirement with attributes and relationships.
    /// This aggregate serves as a proxy/view of a requirement from the core domain
    /// with additional analysis data.
    /// </summary>
    public class AnalyzedRequirement : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public Guid RequirementId { get; private set; } // Reference to core requirement
        public string RequirementTitle { get; private set; } // Cached for performance
        public ImpactAssessmentResult? ImpactAssessment { get; private set; }
        public DateTime? LastAnalyzedAt { get; private set; }
        public Guid? LastAnalyzedBy { get; private set; }

        // Collections
        private readonly List<RequirementAttributeInstance> _attributes;
        private readonly List<RelationshipLink> _relationships;

        public IReadOnlyCollection<RequirementAttributeInstance> Attributes => _attributes.AsReadOnly();
        public IReadOnlyCollection<RelationshipLink> Relationships => _relationships.AsReadOnly();

        protected AnalyzedRequirement()
        {
            _attributes = new List<RequirementAttributeInstance>();
            _relationships = new List<RelationshipLink>();
        }

        public static AnalyzedRequirement Create(
            Guid id,
            Guid requirementId,
            string requirementTitle,
            Guid? tenantId,
            Guid analyzedBy,
            DateTime analyzedAt)
        {
            var analyzedRequirement = new AnalyzedRequirement
            {
                Id = id,
                RequirementId = requirementId,
                TenantId = tenantId,
                LastAnalyzedBy = analyzedBy,
                LastAnalyzedAt = analyzedAt
            };

            analyzedRequirement.SetRequirementTitle(requirementTitle);
            return analyzedRequirement;
        }

        public void SetAttribute(
            string attributeName,
            AttributeType attributeType,
            AttributeValue attributeValue,
            Guid userId,
            DateTime timestamp)
        {
            var existingAttribute = _attributes.FirstOrDefault(a => a.AttributeName == attributeName);
            
            if (existingAttribute != null)
            {
                existingAttribute.UpdateValue(attributeValue);
            }
            else
            {
                var newAttribute = new RequirementAttributeInstance(
                    GuidGenerator.Create(),
                    Id,
                    attributeName,
                    attributeType,
                    attributeValue,
                    userId,
                    timestamp
                );
                _attributes.Add(newAttribute);
            }

            LastAnalyzedAt = timestamp;
            LastAnalyzedBy = userId;
        }

        public void RemoveAttribute(string attributeName)
        {
            var attribute = _attributes.FirstOrDefault(a => a.AttributeName == attributeName);
            if (attribute != null)
            {
                _attributes.Remove(attribute);
            }
        }

        public RequirementAttributeInstance? GetAttribute(string attributeName)
        {
            return _attributes.FirstOrDefault(a => a.AttributeName == attributeName);
        }

        public void AddRelationship(
            Guid targetRequirementId,
            RelationshipType relationshipType,
            string? description,
            Guid userId,
            DateTime timestamp)
        {
            // Check for existing relationship
            var existingRelationship = _relationships.FirstOrDefault(r => 
                r.TargetRequirementId == targetRequirementId && 
                r.RelationshipType == relationshipType);

            if (existingRelationship != null)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:RelationshipAlreadyExists");
            }

            var relationship = new RelationshipLink(
                GuidGenerator.Create(),
                Id,
                targetRequirementId,
                relationshipType,
                description,
                userId,
                timestamp
            );

            _relationships.Add(relationship);

            // Publish domain event
            AddDistributedEvent(new RequirementRelationshipEstablishedEto
            {
                SourceRequirementId = Id,
                TargetRequirementId = targetRequirementId,
                RelationshipType = relationshipType,
                Description = description,
                TenantId = TenantId
            });
        }

        public void RemoveRelationship(Guid relationshipId)
        {
            var relationship = _relationships.FirstOrDefault(r => r.Id == relationshipId);
            if (relationship != null)
            {
                _relationships.Remove(relationship);
            }
        }

        public void SetImpactAssessment(ImpactAssessmentResult impactAssessment, Guid userId, DateTime timestamp)
        {
            ImpactAssessment = Check.NotNull(impactAssessment, nameof(impactAssessment));
            LastAnalyzedAt = timestamp;
            LastAnalyzedBy = userId;
        }

        public void UpdateRequirementTitle(string newTitle)
        {
            SetRequirementTitle(newTitle);
        }

        private void SetRequirementTitle(string title)
        {
            RequirementTitle = Check.NotNullOrWhiteSpace(title, nameof(title));
        }

        public List<RelationshipLink> GetRelationshipsByType(RelationshipType type)
        {
            return _relationships.Where(r => r.RelationshipType == type).ToList();
        }

        public bool HasAttributeOfType(AttributeType type)
        {
            return _attributes.Any(a => a.AttributeType == type);
        }

        public decimal? GetNumericAttributeValue(string attributeName)
        {
            var attribute = GetAttribute(attributeName);
            return attribute?.AttributeValue.NumericValue;
        }

        public string? GetStringAttributeValue(string attributeName)
        {
            var attribute = GetAttribute(attributeName);
            return attribute?.AttributeValue.StringValue;
        }
    }
}
