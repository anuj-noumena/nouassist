using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Enums;
using NouAssist.ContractPortfolioManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.ContractPortfolioManagement.Portfolios.Dtos;

/// <summary>
/// DTO for portfolio goal
/// </summary>
public class PortfolioGoalDto
{
    /// <summary>
    /// Goal title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Goal description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Target date for the goal
    /// </summary>
    public DateTime TargetDate { get; set; }

    /// <summary>
    /// Whether the goal has been achieved
    /// </summary>
    public bool IsAchieved { get; set; }
}

/// <summary>
/// DTO for creating portfolio goal
/// </summary>
public class CreatePortfolioGoalDto
{
    /// <summary>
    /// Goal title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Goal description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Target date for the goal
    /// </summary>
    public DateTime TargetDate { get; set; }
}

/// <summary>
/// DTO for performance metric
/// </summary>
public class PerformanceMetricDto
{
    /// <summary>
    /// Metric name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Metric description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Target value
    /// </summary>
    public decimal Target { get; set; }

    /// <summary>
    /// Actual value
    /// </summary>
    public decimal Actual { get; set; }

    /// <summary>
    /// Unit of measurement
    /// </summary>
    public string Unit { get; set; } = null!;

    /// <summary>
    /// When the metric was last updated
    /// </summary>
    public DateTime LastUpdated { get; set; }

    /// <summary>
    /// Progress percentage
    /// </summary>
    public decimal Progress => Target == 0 ? 0 : Math.Min(100, (Actual / Target) * 100);
}

/// <summary>
/// DTO for creating performance metric
/// </summary>
public class CreatePerformanceMetricDto
{
    /// <summary>
    /// Metric name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Metric description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Target value
    /// </summary>
    public decimal Target { get; set; }

    /// <summary>
    /// Actual value
    /// </summary>
    public decimal Actual { get; set; }

    /// <summary>
    /// Unit of measurement
    /// </summary>
    public string Unit { get; set; } = null!;
}

/// <summary>
/// DTO for updating performance metric
/// </summary>
public class UpdatePerformanceMetricDto
{
    /// <summary>
    /// Metric name to update
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// New actual value
    /// </summary>
    public decimal Actual { get; set; }
}

/// <summary>
/// DTO for portfolio item
/// </summary>
public class PortfolioItemDto : EntityDto<Guid>
{
    /// <summary>
    /// Portfolio this item belongs to
    /// </summary>
    public Guid PortfolioId { get; set; }

    /// <summary>
    /// Item reference
    /// </summary>
    public ItemReferenceDto Reference { get; set; } = null!;

    /// <summary>
    /// Category ID (optional)
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// Item progress (0-100%)
    /// </summary>
    public decimal? Progress { get; set; }

    /// <summary>
    /// Notes about the item
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// When the item was added to the portfolio
    /// </summary>
    public DateTime AddedDate { get; set; }

    /// <summary>
    /// When the item was last updated
    /// </summary>
    public DateTime? LastUpdatedDate { get; set; }

    /// <summary>
    /// Category name (if available)
    /// </summary>
    public string? CategoryName { get; set; }
}

/// <summary>
/// DTO for adding portfolio item
/// </summary>
public class AddPortfolioItemDto
{
    /// <summary>
    /// Item ID
    /// </summary>
    public Guid ItemId { get; set; }

    /// <summary>
    /// Item type
    /// </summary>
    public PortfolioItemType ItemType { get; set; }

    /// <summary>
    /// Item name
    /// </summary>
    public string ItemName { get; set; } = null!;

    /// <summary>
    /// Category ID (optional)
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// Notes about the item
    /// </summary>
    public string? Notes { get; set; }
}

/// <summary>
/// DTO for updating portfolio item
/// </summary>
public class UpdatePortfolioItemDto
{
    /// <summary>
    /// Category ID (optional)
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// Item progress (0-100%)
    /// </summary>
    public decimal? Progress { get; set; }

    /// <summary>
    /// Notes about the item
    /// </summary>
    public string? Notes { get; set; }
}

/// <summary>
/// DTO for item reference
/// </summary>
public class ItemReferenceDto
{
    /// <summary>
    /// Item ID
    /// </summary>
    public Guid ItemId { get; set; }

    /// <summary>
    /// Item type
    /// </summary>
    public PortfolioItemType ItemType { get; set; }

    /// <summary>
    /// Item name
    /// </summary>
    public string ItemName { get; set; } = null!;
}

/// <summary>
/// DTO for portfolio category
/// </summary>
public class PortfolioCategoryDto : EntityDto<Guid>
{
    /// <summary>
    /// Portfolio this category belongs to
    /// </summary>
    public Guid PortfolioId { get; set; }

    /// <summary>
    /// Category name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Category description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Color code for display
    /// </summary>
    public string? ColorCode { get; set; }

    /// <summary>
    /// Number of items in this category
    /// </summary>
    public int ItemCount { get; set; }
}

/// <summary>
/// DTO for creating portfolio category
/// </summary>
public class CreatePortfolioCategoryDto
{
    /// <summary>
    /// Category name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Category description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Color code for display
    /// </summary>
    public string? ColorCode { get; set; }
}

/// <summary>
/// DTO for portfolio risk
/// </summary>
public class PortfolioRiskDto : EntityDto<Guid>
{
    /// <summary>
    /// Portfolio this risk belongs to
    /// </summary>
    public Guid PortfolioId { get; set; }

    /// <summary>
    /// Risk title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Risk assessment data
    /// </summary>
    public RiskAssessmentDataDto Assessment { get; set; } = null!;
}

/// <summary>
/// DTO for creating portfolio risk
/// </summary>
public class CreatePortfolioRiskDto
{
    /// <summary>
    /// Risk title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Risk level
    /// </summary>
    public RiskLevel Level { get; set; }

    /// <summary>
    /// Risk description
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Risk mitigation plan
    /// </summary>
    public string? MitigationPlan { get; set; }

    /// <summary>
    /// Probability (0-1)
    /// </summary>
    public decimal Probability { get; set; }

    /// <summary>
    /// Impact (1-10)
    /// </summary>
    public decimal Impact { get; set; }
}

/// <summary>
/// DTO for risk assessment data
/// </summary>
public class RiskAssessmentDataDto
{
    /// <summary>
    /// Risk level
    /// </summary>
    public RiskLevel Level { get; set; }

    /// <summary>
    /// Risk description
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Risk mitigation plan
    /// </summary>
    public string? MitigationPlan { get; set; }

    /// <summary>
    /// Probability (0-1)
    /// </summary>
    public decimal Probability { get; set; }

    /// <summary>
    /// Impact (1-10)
    /// </summary>
    public decimal Impact { get; set; }

    /// <summary>
    /// Assessment date
    /// </summary>
    public DateTime AssessmentDate { get; set; }

    /// <summary>
    /// Risk score calculation
    /// </summary>
    public decimal RiskScore => Probability * Impact;
}

/// <summary>
/// DTO for portfolio compliance record
/// </summary>
public class PortfolioComplianceRecordDto : EntityDto<Guid>
{
    /// <summary>
    /// Portfolio this record belongs to
    /// </summary>
    public Guid PortfolioId { get; set; }

    /// <summary>
    /// Compliance rule
    /// </summary>
    public ComplianceRuleDto Rule { get; set; } = null!;

    /// <summary>
    /// Whether the portfolio is compliant
    /// </summary>
    public bool IsCompliant { get; set; }

    /// <summary>
    /// Description of compliance evidence
    /// </summary>
    public string? EvidenceDescription { get; set; }

    /// <summary>
    /// When the assessment was performed
    /// </summary>
    public DateTime AssessmentDate { get; set; }
}

/// <summary>
/// DTO for compliance rule
/// </summary>
public class ComplianceRuleDto
{
    /// <summary>
    /// Rule code
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    /// Rule name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Rule description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Rule requirement text
    /// </summary>
    public string RequirementText { get; set; } = null!;

    /// <summary>
    /// Whether compliance is mandatory
    /// </summary>
    public bool IsMandatory { get; set; }
}

/// <summary>
/// DTO for portfolio sharing link
/// </summary>
public class SharingLinkDto
{
    /// <summary>
    /// Link ID
    /// </summary>
    public Guid LinkId { get; set; }

    /// <summary>
    /// Access token
    /// </summary>
    public string Token { get; set; } = null!;

    /// <summary>
    /// Link expiration date
    /// </summary>
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    /// Access permissions
    /// </summary>
    public AccessPermissionsDto Permissions { get; set; } = null!;

    /// <summary>
    /// Whether the link has expired
    /// </summary>
    public bool IsExpired { get; set; }
}

/// <summary>
/// DTO for creating sharing link
/// </summary>
public class CreateSharingLinkDto
{
    /// <summary>
    /// Link expiration date
    /// </summary>
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    /// Whether the user can view
    /// </summary>
    public bool CanView { get; set; } = true;

    /// <summary>
    /// Whether the user can edit
    /// </summary>
    public bool CanEdit { get; set; }

    /// <summary>
    /// Whether the user can share
    /// </summary>
    public bool CanShare { get; set; }

    /// <summary>
    /// Whether the user can delete
    /// </summary>
    public bool CanDelete { get; set; }
}

/// <summary>
/// DTO for access permissions
/// </summary>
public class AccessPermissionsDto
{
    /// <summary>
    /// Whether the user can view
    /// </summary>
    public bool CanView { get; set; }

    /// <summary>
    /// Whether the user can edit
    /// </summary>
    public bool CanEdit { get; set; }

    /// <summary>
    /// Whether the user can share
    /// </summary>
    public bool CanShare { get; set; }

    /// <summary>
    /// Whether the user can delete
    /// </summary>
    public bool CanDelete { get; set; }
}
