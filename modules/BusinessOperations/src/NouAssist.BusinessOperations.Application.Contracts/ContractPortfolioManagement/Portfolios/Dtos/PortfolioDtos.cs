using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.ContractPortfolioManagement.Portfolios.Dtos;

/// <summary>
/// Base DTO for Portfolio operations
/// </summary>
public abstract class PortfolioDtoBase
{
    /// <summary>
    /// Portfolio name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Portfolio description
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// DTO for portfolio details
/// </summary>
public class PortfolioDto : AuditedEntityDto<Guid>
{
    /// <summary>
    /// Portfolio name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Portfolio description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Whether the portfolio is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Portfolio goals
    /// </summary>
    public List<PortfolioGoalDto> Goals { get; set; } = new();

    /// <summary>
    /// Performance metrics
    /// </summary>
    public List<PerformanceMetricDto> Metrics { get; set; } = new();

    /// <summary>
    /// Portfolio items
    /// </summary>
    public List<PortfolioItemDto> Items { get; set; } = new();

    /// <summary>
    /// Portfolio categories
    /// </summary>
    public List<PortfolioCategoryDto> Categories { get; set; } = new();
}

/// <summary>
/// DTO for portfolio list results
/// </summary>
public class PortfolioListDto : EntityDto<Guid>
{
    /// <summary>
    /// Portfolio name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Portfolio description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Whether the portfolio is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Number of items in the portfolio
    /// </summary>
    public int ItemCount { get; set; }

    /// <summary>
    /// Creation date of the portfolio
    /// </summary>
    public DateTime CreationTime { get; set; }
}

/// <summary>
/// DTO for creating a portfolio
/// </summary>
public class CreatePortfolioDto : PortfolioDtoBase
{
}

/// <summary>
/// DTO for updating a portfolio
/// </summary>
public class UpdatePortfolioDto : PortfolioDtoBase
{
    /// <summary>
    /// Whether the portfolio is active
    /// </summary>
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for querying portfolios
/// </summary>
public class GetPortfolioListDto : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// General filter text (searches name, description)
    /// </summary>
    public string? Filter { get; set; }

    /// <summary>
    /// Filter by active status
    /// </summary>
    public bool? IsActive { get; set; }
}

/// <summary>
/// DTO for portfolio performance report
/// </summary>
public class PortfolioPerformanceReportDto : EntityDto<Guid>
{
    /// <summary>
    /// Portfolio name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Overall progress percentage
    /// </summary>
    public decimal OverallProgress { get; set; }

    /// <summary>
    /// Risk score
    /// </summary>
    public decimal RiskScore { get; set; }

    /// <summary>
    /// Compliance percentage
    /// </summary>
    public decimal CompliancePercentage { get; set; }

    /// <summary>
    /// Number of items in the portfolio
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Performance metrics
    /// </summary>
    public List<PerformanceMetricDto> Metrics { get; set; } = new();

    /// <summary>
    /// Risk assessments
    /// </summary>
    public List<PortfolioRiskDto> Risks { get; set; } = new();
}
