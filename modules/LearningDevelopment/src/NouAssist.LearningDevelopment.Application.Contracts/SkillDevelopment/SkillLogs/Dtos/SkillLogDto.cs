using System;
using System.Collections.Generic;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.SkillDevelopment.SkillLogs.Dtos
{
    public class SkillLogDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public Guid SkillId { get; set; }
        public string SkillTitle { get; set; }
        public SkillLogType Type { get; set; }
        public string Content { get; set; }
        public int? ProficiencyScore { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
        public List<string> Tags { get; set; }
    }

    public class CreateSkillLogDto
    {
        public Guid SkillId { get; set; }
        public SkillLogType Type { get; set; }
        public string Content { get; set; }
        public int? ProficiencyScore { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
        public List<string> Tags { get; set; }
    }

    public class UpdateSkillLogDto
    {
        public string Content { get; set; }
        public int? ProficiencyScore { get; set; }
        public List<string> Tags { get; set; }
    }

    public class GetSkillLogListDto : PagedAndSortedResultRequestDto
    {
        public Guid? SkillId { get; set; }
        public SkillLogType? Type { get; set; }
        public string Tag { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
