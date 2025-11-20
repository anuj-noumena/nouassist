using System;
using System.Collections.Generic;
using NouAssist.ContractPortfolioManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.ContractPortfolioManagement.Contracts.Dtos;

/// <summary>
/// Base DTO for Contract operations
/// </summary>
public abstract class ContractDtoBase
{
    /// <summary>
    /// Contract title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Contract reference number
    /// </summary>
    public string ReferenceNumber { get; set; } = null!;

    /// <summary>
    /// Contract description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Effective date of the contract
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// Expiration date of the contract (optional)
    /// </summary>
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Total monetary value of the contract
    /// </summary>
    public decimal TotalValue { get; set; }

    /// <summary>
    /// Currency code (e.g., USD, EUR)
    /// </summary>
    public string Currency { get; set; } = null!;
}

/// <summary>
/// DTO for contract details
/// </summary>
public class ContractDto : AuditedEntityDto<Guid>
{
    /// <summary>
    /// Contract title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Contract reference number
    /// </summary>
    public string ReferenceNumber { get; set; } = null!;

    /// <summary>
    /// Contract description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Current status of the contract
    /// </summary>
    public ContractStatus Status { get; set; }

    /// <summary>
    /// Effective date of the contract
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// Expiration date of the contract (optional)
    /// </summary>
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Total monetary value of the contract
    /// </summary>
    public decimal TotalValue { get; set; }

    /// <summary>
    /// Currency code (e.g., USD, EUR)
    /// </summary>
    public string Currency { get; set; } = null!;

    /// <summary>
    /// Template reference information
    /// </summary>
    public ContractTemplateReferenceDto TemplateReference { get; set; } = null!;

    /// <summary>
    /// Main document information
    /// </summary>
    public DocumentIdDto MainDocument { get; set; } = null!;

    /// <summary>
    /// Latest version of the contract
    /// </summary>
    public ContractVersionDto? LatestVersion { get; set; }

    /// <summary>
    /// Collection of contract parties
    /// </summary>
    public List<ContractPartyDto> Parties { get; set; } = new();

    /// <summary>
    /// Collection of contract alerts
    /// </summary>
    public List<ContractAlertDto> Alerts { get; set; } = new();
}

/// <summary>
/// DTO for contract list results
/// </summary>
public class ContractListDto : EntityDto<Guid>
{
    /// <summary>
    /// Contract title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Contract reference number
    /// </summary>
    public string ReferenceNumber { get; set; } = null!;

    /// <summary>
    /// Current status of the contract
    /// </summary>
    public ContractStatus Status { get; set; }

    /// <summary>
    /// Effective date of the contract
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// Expiration date of the contract (optional)
    /// </summary>
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Total monetary value of the contract
    /// </summary>
    public decimal TotalValue { get; set; }

    /// <summary>
    /// Currency code (e.g., USD, EUR)
    /// </summary>
    public string Currency { get; set; } = null!;

    /// <summary>
    /// Creation date of the contract
    /// </summary>
    public DateTime CreationTime { get; set; }
}

/// <summary>
/// DTO for creating a contract
/// </summary>
public class CreateContractDto : ContractDtoBase
{
    /// <summary>
    /// Template information
    /// </summary>
    public CreateContractTemplateReferenceDto TemplateReference { get; set; } = null!;

    /// <summary>
    /// Main document ID
    /// </summary>
    public CreateDocumentIdDto MainDocument { get; set; } = null!;
}

/// <summary>
/// DTO for updating a contract
/// </summary>
public class UpdateContractDto : ContractDtoBase
{
}

/// <summary>
/// DTO for querying contracts
/// </summary>
public class GetContractListDto : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// General filter text (searches title, reference number)
    /// </summary>
    public string? Filter { get; set; }

    /// <summary>
    /// Filter by contract status
    /// </summary>
    public ContractStatus? Status { get; set; }

    /// <summary>
    /// Filter by effective date after
    /// </summary>
    public DateTime? EffectiveDateMin { get; set; }

    /// <summary>
    /// Filter by effective date before
    /// </summary>
    public DateTime? EffectiveDateMax { get; set; }

    /// <summary>
    /// Filter by total value minimum
    /// </summary>
    public decimal? TotalValueMin { get; set; }

    /// <summary>
    /// Filter by total value maximum
    /// </summary>
    public decimal? TotalValueMax { get; set; }
}

/// <summary>
/// DTO for changing contract status
/// </summary>
public class ChangeContractStatusDto
{
    /// <summary>
    /// New status to set
    /// </summary>
    public ContractStatus NewStatus { get; set; }
}
