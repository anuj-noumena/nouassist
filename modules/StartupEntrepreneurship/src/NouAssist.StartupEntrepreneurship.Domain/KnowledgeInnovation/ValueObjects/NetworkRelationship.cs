using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects
{
    /// <summary>
    /// Value object representing a network relationship
    /// </summary>
    public class NetworkRelationship : ValueObject
    {
        public Guid RelatedEntityId { get; private set; }
        public string RelatedEntityName { get; private set; }
        public NetworkRelationshipType RelationshipType { get; private set; }
        public string Description { get; private set; }
        public DateTime EstablishedDate { get; private set; }

        private NetworkRelationship() { }

        public NetworkRelationship(
            Guid relatedEntityId, 
            string relatedEntityName, 
            NetworkRelationshipType relationshipType, 
            string description, 
            DateTime establishedDate)
        {
            RelatedEntityId = relatedEntityId;
            
            RelatedEntityName = Check.NotNullOrWhiteSpace(
                relatedEntityName, 
                nameof(relatedEntityName), 
                maxLength: KnowledgeInnovationConsts.DefaultLengths.Name);
            
            RelationshipType = relationshipType;
            Description = description;
            EstablishedDate = establishedDate;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return RelatedEntityId;
            yield return RelatedEntityName;
            yield return RelationshipType;
            yield return Description;
            yield return EstablishedDate;
        }
    }
}
