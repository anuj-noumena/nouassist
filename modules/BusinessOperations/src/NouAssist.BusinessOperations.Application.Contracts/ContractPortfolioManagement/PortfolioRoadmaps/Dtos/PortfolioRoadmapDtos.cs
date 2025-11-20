using System;
using System.Collections.Generic;
using NouAssist.ContractPortfolioManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.ContractPortfolioManagement.PortfolioRoadmaps.Dtos;

/// <summary>
/// Base DTO for PortfolioRoadmap operations
/// </summary>
public abstract class PortfolioRoadmapDtoBase
{
    /// <summary>
    /// Roadmap title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Roadmap description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Portfolio the roadmap belongs to
    /// </summary>
    public Guid PortfolioId { get; set; }

    /// <summary>
    /// Start date of the roadmap
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End date of the roadmap
    /// </summary>
    public DateTime EndDate { get; set; }
}

/// <summary>
/// DTO for roadmap details
/// </summary>
public class PortfolioRoadmapDto : AuditedEntityDto<Guid>
{
    /// <summary>
    /// Roadmap title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Roadmap description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Portfolio the roadmap belongs to
    /// </summary>
    public Guid PortfolioId { get; set; }

    /// <summary>
    /// Start date of the roadmap
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End date of the roadmap
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Overall progress percentage
    /// </summary>
    public double OverallProgress { get; set; }

    /// <summary>
    /// Collection of roadmap milestones
    /// </summary>
    public List<RoadmapMilestoneDto> Milestones { get; set; } = new();

    /// <summary>
    /// Collection of cross-item dependencies
    /// </summary>
    public List<CrossItemDependencyDto> Dependencies { get; set; } = new();
}

/// <summary>
/// DTO for roadmap list results
/// </summary>
public class PortfolioRoadmapListDto : EntityDto<Guid>
{
    /// <summary>
    /// Roadmap title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Roadmap description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Portfolio the roadmap belongs to
    /// </summary>
    public Guid PortfolioId { get; set; }

    /// <summary>
    /// Portfolio name
    /// </summary>
    public string PortfolioName { get; set; } = null!;

    /// <summary>
    /// Start date of the roadmap
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End date of the roadmap
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Number of milestones in the roadmap
    /// </summary>
    public int MilestoneCount { get; set; }

    /// <summary>
    /// Overall progress percentage
    /// </summary>
    public double OverallProgress { get; set; }
}

/// <summary>
/// DTO for creating a roadmap
/// </summary>
public class CreatePortfolioRoadmapDto : PortfolioRoadmapDtoBase
{
}

/// <summary>
/// DTO for updating a roadmap
/// </summary>
public class UpdatePortfolioRoadmapDto : PortfolioRoadmapDtoBase
{
}

/// <summary>
/// DTO for querying roadmaps
/// </summary>
public class GetPortfolioRoadmapListDto : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// General filter text (searches title, description)
    /// </summary>
    public string? Filter { get; set; }

    /// <summary>
    /// Filter by portfolio ID
    /// </summary>
    public Guid? PortfolioId { get; set; }

    /// <summary>
    /// Filter by start date after
    /// </summary>
    public DateTime? StartDateMin { get; set; }

    /// <summary>
    /// Filter by end date before
    /// </summary>
    public DateTime? EndDateMax { get; set; }
}

/// <summary>
/// DTO for querying milestones in date range
/// </summary>
public class GetMilestonesInRangeDto
{
    /// <summary>
    /// Start date of the range
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End date of the range
    /// </summary>
    public DateTime EndDate { get; set; }
}

/// <summary>
/// DTO for roadmap milestone
/// </summary>
public class RoadmapMilestoneDto : EntityDto<Guid>
{
    /// <summary>
    /// Roadmap this milestone belongs to
    /// </summary>
    public Guid RoadmapId { get; set; }

    /// <summary>
    /// Milestone title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Milestone description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Milestone date information
    /// </summary>
    public MilestoneDateDto Date { get; set; } = null!;

    /// <summary>
    /// Portfolio item this milestone is associated with (optional)
    /// </summary>
    public Guid? PortfolioItemId { get; set; }

    /// <summary>
    /// Skill requirements for this milestone
    /// </summary>
    public List<ResourceSkillRequirementDto> SkillRequirements { get; set; } = new();

    /// <summary>
    /// Dependencies where this milestone is a source
    /// </summary>
    public List<CrossItemDependencyDto> OutgoingDependencies { get; set; } = new();

    /// <summary>
    /// Dependencies where this milestone is a target
    /// </summary>
    public List<CrossItemDependencyDto> IncomingDependencies { get; set; } = new();
}

/// <summary>
/// DTO for creating roadmap milestone
/// </summary>
public class CreateRoadmapMilestoneDto
{
    /// <summary>
    /// Milestone title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Milestone description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Planned date for the milestone
    /// </summary>
    public DateTime PlannedDate { get; set; }

    /// <summary>
    /// Portfolio item this milestone is associated with (optional)
    /// </summary>
    public Guid? PortfolioItemId { get; set; }
}

/// <summary>
/// DTO for updating roadmap milestone
/// </summary>
public class UpdateRoadmapMilestoneDto
{
    /// <summary>
    /// Milestone title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Milestone description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Planned date for the milestone
    /// </summary>
    public DateTime PlannedDate { get; set; }

    /// <summary>
    /// Portfolio item this milestone is associated with (optional)
    /// </summary>
    public Guid? PortfolioItemId { get; set; }
}

/// <summary>
/// DTO for updating milestone completion status
/// </summary>
public class UpdateMilestoneCompletionDto
{
    /// <summary>
    /// Whether the milestone is completed
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Actual completion date (if completed)
    /// </summary>
    public DateTime? CompletionDate { get; set; }
}

/// <summary>
/// DTO for milestone date
/// </summary>
public class MilestoneDateDto
{
    /// <summary>
    /// Planned date for the milestone
    /// </summary>
    public DateTime PlannedDate { get; set; }

    /// <summary>
    /// Actual date when milestone was completed
    /// </summary>
    public DateTime? ActualDate { get; set; }

    /// <summary>
    /// Whether the milestone is completed
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Days variance between planned and actual (if completed)
    /// </summary>
    public int? DaysVariance => ActualDate.HasValue ? (int)(ActualDate.Value - PlannedDate).TotalDays : null;
}

/// <summary>
/// DTO for cross-item dependency
/// </summary>
public class CrossItemDependencyDto : EntityDto<Guid>
{
    /// <summary>
    /// Roadmap this dependency belongs to
    /// </summary>
    public Guid RoadmapId { get; set; }

    /// <summary>
    /// Source milestone ID
    /// </summary>
    public Guid SourceMilestoneId { get; set; }

    /// <summary>
    /// Source milestone title
    /// </summary>
    public string? SourceMilestoneTitle { get; set; }

    /// <summary>
    /// Target milestone ID
    /// </summary>
    public Guid TargetMilestoneId { get; set; }

    /// <summary>
    /// Target milestone title
    /// </summary>
    public string? TargetMilestoneTitle { get; set; }

    /// <summary>
    /// Dependency type
    /// </summary>
    public DependencyType Type { get; set; }
}

/// <summary>
/// DTO for creating cross-item dependency
/// </summary>
public class CreateCrossItemDependencyDto
{
    /// <summary>
    /// Source milestone ID
    /// </summary>
    public Guid SourceMilestoneId { get; set; }

    /// <summary>
    /// Target milestone ID
    /// </summary>
    public Guid TargetMilestoneId { get; set; }

    /// <summary>
    /// Dependency type
    /// </summary>
    public DependencyType Type { get; set; }
}

/// <summary>
/// DTO for resource skill requirement
/// </summary>
public class ResourceSkillRequirementDto
{
    /// <summary>
    /// Skill name
    /// </summary>
    public string SkillName { get; set; } = null!;

    /// <summary>
    /// Required level (1-5)
    /// </summary>
    public int RequiredLevel { get; set; }

    /// <summary>
    /// Whether the skill is mandatory
    /// </summary>
    public bool IsMandatory { get; set; }

    /// <summary>
    /// Description of the requirement
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// DTO for creating resource skill requirement
/// </summary>
public class CreateResourceSkillRequirementDto
{
    /// <summary>
    /// Milestone ID
    /// </summary>
    public Guid MilestoneId { get; set; }

    /// <summary>
    /// Skill name
    /// </summary>
    public string SkillName { get; set; } = null!;

    /// <summary>
    /// Required level (1-5)
    /// </summary>
    public int RequiredLevel { get; set; }

    /// <summary>
    /// Whether the skill is mandatory
    /// </summary>
    public bool IsMandatory { get; set; }

    /// <summary>
    /// Description of the requirement
    /// </summary>
    public string? Description { get; set; }
}
