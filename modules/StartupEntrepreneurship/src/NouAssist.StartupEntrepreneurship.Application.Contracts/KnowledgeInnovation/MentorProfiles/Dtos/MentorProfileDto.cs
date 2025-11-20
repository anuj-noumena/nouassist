using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.MentorProfiles.Dtos
{
    public class MentorProfileDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Expertise { get; set; }
        public int ExperienceYears { get; set; }
        public List<string> Tags { get; set; }
        public List<NetworkRelationshipDto> Relationships { get; set; }
        public List<AdviceRecordDto> AdviceRecords { get; set; }
    }

    public class NetworkRelationshipDto
    {
        public Guid RelatedEntityId { get; set; }
        public string RelatedEntityName { get; set; }
        public NetworkRelationshipType RelationshipType { get; set; }
        public string Description { get; set; }
        public DateTime EstablishedDate { get; set; }
    }

    public class AdviceRecordDto
    {
        public string Topic { get; set; }
        public string Content { get; set; }
        public DateTime GivenOn { get; set; }
        public List<string> Tags { get; set; }
    }
}
