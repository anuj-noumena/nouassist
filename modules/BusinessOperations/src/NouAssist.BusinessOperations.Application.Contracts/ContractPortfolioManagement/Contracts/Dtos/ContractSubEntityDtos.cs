using System;
using System.Collections.Generic;
using NouAssist.ContractPortfolioManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.ContractPortfolioManagement.Contracts.Dtos;

/// <summary>
/// DTO for template reference information
/// </summary>
public class ContractTemplateReferenceDto
{
    /// <summary>
    /// Template ID
    /// </summary>
    public Guid TemplateId { get; set; }

    /// <summary>
    /// Template name
    /// </summary>
    public string TemplateName { get; set; } = null!;

    /// <summary>
    /// Template version
    /// </summary>
    public string Version { get; set; } = null!;
}

/// <summary>
/// DTO for creating template reference
/// </summary>
public class CreateContractTemplateReferenceDto
{
    /// <summary>
    /// Template ID
    /// </summary>
    public Guid TemplateId { get; set; }

    /// <summary>
    /// Template name
    /// </summary>
    public string TemplateName { get; set; } = null!;

    /// <summary>
    /// Template version
    /// </summary>
    public string Version { get; set; } = null!;
}

/// <summary>
/// DTO for document identifier
/// </summary>
public class DocumentIdDto
{
    /// <summary>
    /// Document ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Repository type where document is stored
    /// </summary>
    public string RepositoryType { get; set; } = null!;
}

/// <summary>
/// DTO for creating document identifier
/// </summary>
public class CreateDocumentIdDto
{
    /// <summary>
    /// Document ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Repository type where document is stored
    /// </summary>
    public string RepositoryType { get; set; } = null!;
}

/// <summary>
/// DTO for contract version
/// </summary>
public class ContractVersionDto : EntityDto<Guid>
{
    /// <summary>
    /// Contract this version belongs to
    /// </summary>
    public Guid ContractId { get; set; }

    /// <summary>
    /// Version metadata
    /// </summary>
    public VersionMetadataDto Metadata { get; set; } = null!;

    /// <summary>
    /// Document identifier
    /// </summary>
    public DocumentIdDto DocumentId { get; set; } = null!;

    /// <summary>
    /// When this version was created
    /// </summary>
    public DateTime CreationTime { get; set; }
}

/// <summary>
/// DTO for creating contract version
/// </summary>
public class CreateContractVersionDto
{
    /// <summary>
    /// Version label
    /// </summary>
    public string VersionLabel { get; set; } = null!;

    /// <summary>
    /// Reason for creating this version
    /// </summary>
    public string? ChangeReason { get; set; }

    /// <summary>
    /// Document identifier
    /// </summary>
    public CreateDocumentIdDto DocumentId { get; set; } = null!;
}

/// <summary>
/// DTO for version metadata
/// </summary>
public class VersionMetadataDto
{
    /// <summary>
    /// Version number (1, 2, 3, etc.)
    /// </summary>
    public int VersionNumber { get; set; }

    /// <summary>
    /// Version label
    /// </summary>
    public string VersionLabel { get; set; } = null!;

    /// <summary>
    /// Reason for this version
    /// </summary>
    public string? ChangeReason { get; set; }
}

/// <summary>
/// DTO for contract party
/// </summary>
public class ContractPartyDto : EntityDto<Guid>
{
    /// <summary>
    /// Contract this party belongs to
    /// </summary>
    public Guid ContractId { get; set; }

    /// <summary>
    /// Party name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Party identifier (business ID, SSN, etc.)
    /// </summary>
    public string Identifier { get; set; } = null!;

    /// <summary>
    /// Contact information
    /// </summary>
    public string? ContactInfo { get; set; }

    /// <summary>
    /// Type of party
    /// </summary>
    public ContractPartyType PartyType { get; set; }
}

/// <summary>
/// DTO for creating contract party
/// </summary>
public class CreateContractPartyDto
{
    /// <summary>
    /// Party name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Party identifier (business ID, SSN, etc.)
    /// </summary>
    public string Identifier { get; set; } = null!;

    /// <summary>
    /// Contact information
    /// </summary>
    public string? ContactInfo { get; set; }

    /// <summary>
    /// Type of party
    /// </summary>
    public ContractPartyType PartyType { get; set; }
}

/// <summary>
/// DTO for contract amendment
/// </summary>
public class ContractAmendmentDto : EntityDto<Guid>
{
    /// <summary>
    /// Contract this amendment belongs to
    /// </summary>
    public Guid ContractId { get; set; }

    /// <summary>
    /// Amendment details
    /// </summary>
    public AmendmentDetailsDto Details { get; set; } = null!;

    /// <summary>
    /// Document identifier
    /// </summary>
    public DocumentIdDto DocumentId { get; set; } = null!;

    /// <summary>
    /// When this amendment was created
    /// </summary>
    public DateTime CreationTime { get; set; }
}

/// <summary>
/// DTO for creating contract amendment
/// </summary>
public class CreateContractAmendmentDto
{
    /// <summary>
    /// Amendment title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Amendment description
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// When this amendment takes effect
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// Sections affected by this amendment
    /// </summary>
    public List<string> AffectedSections { get; set; } = new();

    /// <summary>
    /// Document identifier
    /// </summary>
    public CreateDocumentIdDto DocumentId { get; set; } = null!;
}

/// <summary>
/// DTO for amendment details
/// </summary>
public class AmendmentDetailsDto
{
    /// <summary>
    /// Amendment title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Amendment description
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// When this amendment takes effect
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// Sections affected by this amendment
    /// </summary>
    public List<string> AffectedSections { get; set; } = new();
}

/// <summary>
/// DTO for contract alert
/// </summary>
public class ContractAlertDto : EntityDto<Guid>
{
    /// <summary>
    /// Contract this alert belongs to
    /// </summary>
    public Guid ContractId { get; set; }

    /// <summary>
    /// Alert title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Alert description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Type of alert
    /// </summary>
    public ContractAlertType AlertType { get; set; }

    /// <summary>
    /// Alert condition
    /// </summary>
    public AlertConditionDto Condition { get; set; } = null!;

    /// <summary>
    /// Whether the alert has been triggered
    /// </summary>
    public bool IsTriggered { get; set; }

    /// <summary>
    /// When the alert was last triggered
    /// </summary>
    public DateTime? LastTriggeredTime { get; set; }
}

/// <summary>
/// DTO for creating contract alert
/// </summary>
public class CreateContractAlertDto
{
    /// <summary>
    /// Alert title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Alert description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Type of alert
    /// </summary>
    public ContractAlertType AlertType { get; set; }

    /// <summary>
    /// Date for alert trigger
    /// </summary>
    public DateTime TriggerDate { get; set; }

    /// <summary>
    /// Days in advance to trigger alert
    /// </summary>
    public int DaysInAdvance { get; set; }

    /// <summary>
    /// Whether the alert is recurring
    /// </summary>
    public bool IsRecurring { get; set; }

    /// <summary>
    /// Frequency of recurrence (daily, weekly, monthly, etc.)
    /// </summary>
    public string? Frequency { get; set; }
}

/// <summary>
/// DTO for alert condition
/// </summary>
public class AlertConditionDto
{
    /// <summary>
    /// Date for alert trigger
    /// </summary>
    public DateTime TriggerDate { get; set; }

    /// <summary>
    /// Days in advance to trigger alert
    /// </summary>
    public int DaysInAdvance { get; set; }

    /// <summary>
    /// Whether the alert is recurring
    /// </summary>
    public bool IsRecurring { get; set; }

    /// <summary>
    /// Frequency of recurrence (daily, weekly, monthly, etc.)
    /// </summary>
    public string? Frequency { get; set; }
}

/// <summary>
/// DTO for contract clause
/// </summary>
public class ContractClauseDto : EntityDto<Guid>
{
    /// <summary>
    /// Contract this clause belongs to
    /// </summary>
    public Guid ContractId { get; set; }

    /// <summary>
    /// Clause title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Clause content
    /// </summary>
    public string Content { get; set; } = null!;

    /// <summary>
    /// Reference identifier
    /// </summary>
    public string? Reference { get; set; }

    /// <summary>
    /// Whether this clause is mandatory
    /// </summary>
    public bool IsMandatory { get; set; }
}

/// <summary>
/// DTO for creating contract clause
/// </summary>
public class CreateContractClauseDto
{
    /// <summary>
    /// Clause title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Clause content
    /// </summary>
    public string Content { get; set; } = null!;

    /// <summary>
    /// Reference identifier
    /// </summary>
    public string? Reference { get; set; }

    /// <summary>
    /// Whether this clause is mandatory
    /// </summary>
    public bool IsMandatory { get; set; }
}
