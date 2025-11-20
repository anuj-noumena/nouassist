using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.MentorProfiles.Dtos
{
    public class MentorProfileListDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Expertise { get; set; }
        public int ExperienceYears { get; set; }
        public string ProfilePictureUrl { get; set; }
        public List<string> Tags { get; set; }
        public int RelationshipsCount { get; set; }
        public int AdviceCount { get; set; }
    }
}
