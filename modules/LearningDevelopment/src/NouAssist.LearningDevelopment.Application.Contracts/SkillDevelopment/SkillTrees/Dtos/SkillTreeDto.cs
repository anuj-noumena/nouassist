using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.SkillDevelopment.SkillTrees.Dtos
{
    public class SkillTreeDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public Guid SkillId { get; set; }
        public string SkillTitle { get; set; }
        public Dictionary<string, object> Structure { get; set; }
        public string VisualizationUrl { get; set; }
    }

    public class UpdateSkillTreeDto
    {
        public Dictionary<string, object> Structure { get; set; }
        public string VisualizationUrl { get; set; }
    }

    public class SkillTreeNodeDto
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }

    public class SkillTreeEdgeDto
    {
        public Guid From { get; set; }
        public Guid To { get; set; }
        public string Relationship { get; set; }
    }
}
