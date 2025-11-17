using System;
using System.Collections.Generic;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Skills.Dtos
{
    public class SkillDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsCategory { get; set; }
        public string Category { get; set; }
        public ProficiencyLevel TargetProficiency { get; set; }
        public List<Guid> Prerequisites { get; set; }
        public List<string> Resources { get; set; }
        public int EstimatedPracticeHours { get; set; }
        public string DevelopmentGoals { get; set; }
        public int CurrentProficiencyScore { get; set; }
        public DateTime CurrentProficiencyAssessedAt { get; set; }
        public SkillStatus Status { get; set; }
        public List<Guid> SubSkillIds { get; set; }
        public Guid? ActiveDevelopmentSessionId { get; set; }
    }

    public class SkillSummaryDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public SkillStatus Status { get; set; }
        public int CurrentProficiencyScore { get; set; }
        public ProficiencyLevel TargetProficiency { get; set; }
        public bool IsCategory { get; set; }
        public int SubSkillCount { get; set; }
    }

    public class CreateSkillDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsCategory { get; set; }
        public string Category { get; set; }
        public ProficiencyLevel TargetProficiency { get; set; }
        public List<Guid> Prerequisites { get; set; } = new();
        public List<string> Resources { get; set; } = new();
        public int EstimatedPracticeHours { get; set; }
        public string DevelopmentGoals { get; set; }
    }

    public class UpdateSkillDto
    {
        public string Description { get; set; }
        public string Category { get; set; }
        public string DevelopmentGoals { get; set; }
        public List<string> Resources { get; set; }
    }

    public class UpdateSkillProficiencyDto
    {
        public int Score { get; set; }
        public string Notes { get; set; }
    }

    public class GetSkillListDto : PagedAndSortedResultRequestDto
    {
        public SkillStatus? Status { get; set; }
        public string Category { get; set; }
        public bool? IsCategory { get; set; }
        public Guid? ParentId { get; set; }
        public int? MinProficiency { get; set; }
        public int? MaxProficiency { get; set; }
        public string SearchTerm { get; set; }
    }

    public class SkillProgressDto
    {
        public Guid SkillId { get; set; }
        public string Title { get; set; }
        public int CurrentProficiency { get; set; }
        public int ProficiencyGainThisWeek { get; set; }
        public int TotalPracticeHours { get; set; }
        public DateTime LastActivity { get; set; }
        public string NextMilestone { get; set; }
    }
}
