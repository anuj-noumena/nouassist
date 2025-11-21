using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Constants;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Lead.Dtos;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Organization.Dtos
{
    /// <summary>
    /// Data transfer object for Organization entity
    /// </summary>
    public class OrganizationDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// Name of the organization
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Contact information
        /// </summary>
        public ContactInfoDto ContactInfo { get; set; }
        
        /// <summary>
        /// Detailed information about the organization
        /// </summary>
        public OrganizationInfoDto Details { get; set; }
        
        /// <summary>
        /// Unique identifier like tax ID or business registration number
        /// </summary>
        public string RegistrationNumber { get; set; }
        
        /// <summary>
        /// Parent organization ID for hierarchical relationships
        /// </summary>
        public Guid? ParentOrganizationId { get; set; }
        
        /// <summary>
        /// Parent organization name (if available)
        /// </summary>
        public string ParentOrganizationName { get; set; }
        
        /// <summary>
        /// Whether this organization is active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Tags for categorization
        /// </summary>
        public List<string> Tags { get; set; }
    }

    /// <summary>
    /// Data transfer object for OrganizationInfo value object
    /// </summary>
    public class OrganizationInfoDto
    {
        /// <summary>
        /// The type of organization
        /// </summary>
        public OrganizationType Type { get; set; }
        
        /// <summary>
        /// The industry the organization operates in
        /// </summary>
        public string Industry { get; set; }
        
        /// <summary>
        /// The number of employees in the organization
        /// </summary>
        public int? EmployeeCount { get; set; }
        
        /// <summary>
        /// Annual revenue of the organization
        /// </summary>
        public decimal? AnnualRevenue { get; set; }
        
        /// <summary>
        /// Tax ID or business registration number
        /// </summary>
        public string TaxId { get; set; }
        
        /// <summary>
        /// Additional notes about the organization
        /// </summary>
        public string Notes { get; set; }
    }

    /// <summary>
    /// Data transfer object for creating an organization
    /// </summary>
    public class CreateOrganizationDto
    {
        /// <summary>
        /// Name of the organization
        /// </summary>
        [Required]
        [StringLength(CRMConsts.MaxLengths.Name)]
        public string Name { get; set; }
        
        /// <summary>
        /// Contact information
        /// </summary>
        [Required]
        public CreateContactInfoDto ContactInfo { get; set; }
        
        /// <summary>
        /// Detailed information about the organization
        /// </summary>
        [Required]
        public CreateOrganizationInfoDto Details { get; set; }
        
        /// <summary>
        /// Unique identifier like tax ID or business registration number
        /// </summary>
        [StringLength(50)]
        public string RegistrationNumber { get; set; }
        
        /// <summary>
        /// Parent organization ID for hierarchical relationships
        /// </summary>
        public Guid? ParentOrganizationId { get; set; }
        
        /// <summary>
        /// Initial tags (optional)
        /// </summary>
        public List<string> Tags { get; set; }
    }

    /// <summary>
    /// Data transfer object for creating organization information
    /// </summary>
    public class CreateOrganizationInfoDto
    {
        /// <summary>
        /// The type of organization
        /// </summary>
        [Required]
        public OrganizationType Type { get; set; }
        
        /// <summary>
        /// The industry the organization operates in
        /// </summary>
        [StringLength(100)]
        public string Industry { get; set; }
        
        /// <summary>
        /// The number of employees in the organization
        /// </summary>
        public int? EmployeeCount { get; set; }
        
        /// <summary>
        /// Annual revenue of the organization
        /// </summary>
        public decimal? AnnualRevenue { get; set; }
        
        /// <summary>
        /// Tax ID or business registration number
        /// </summary>
        [StringLength(50)]
        public string TaxId { get; set; }
        
        /// <summary>
        /// Additional notes about the organization
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Note)]
        public string Notes { get; set; }
    }

    /// <summary>
    /// Data transfer object for updating an organization
    /// </summary>
    public class UpdateOrganizationDto
    {
        /// <summary>
        /// Name of the organization
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Name)]
        public string Name { get; set; }
        
        /// <summary>
        /// Contact information
        /// </summary>
        public UpdateContactInfoDto ContactInfo { get; set; }
        
        /// <summary>
        /// Detailed information about the organization
        /// </summary>
        public UpdateOrganizationInfoDto Details { get; set; }
        
        /// <summary>
        /// Unique identifier like tax ID or business registration number
        /// </summary>
        [StringLength(50)]
        public string RegistrationNumber { get; set; }
    }

    /// <summary>
    /// Data transfer object for updating organization information
    /// </summary>
    public class UpdateOrganizationInfoDto
    {
        /// <summary>
        /// The type of organization
        /// </summary>
        public OrganizationType? Type { get; set; }
        
        /// <summary>
        /// The industry the organization operates in
        /// </summary>
        [StringLength(100)]
        public string Industry { get; set; }
        
        /// <summary>
        /// The number of employees in the organization
        /// </summary>
        public int? EmployeeCount { get; set; }
        
        /// <summary>
        /// Annual revenue of the organization
        /// </summary>
        public decimal? AnnualRevenue { get; set; }
        
        /// <summary>
        /// Tax ID or business registration number
        /// </summary>
        [StringLength(50)]
        public string TaxId { get; set; }
        
        /// <summary>
        /// Additional notes about the organization
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Note)]
        public string Notes { get; set; }
    }

    /// <summary>
    /// Data transfer object for organization list filtering and pagination
    /// </summary>
    public class GetOrganizationListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to search in organization name, email, and industry
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by organization type
        /// </summary>
        public OrganizationType? Type { get; set; }
        
        /// <summary>
        /// Filter by active status
        /// </summary>
        public bool? IsActive { get; set; }
        
        /// <summary>
        /// Filter by tags (comma-separated)
        /// </summary>
        public string Tags { get; set; }
        
        /// <summary>
        /// Filter by industry
        /// </summary>
        public string Industry { get; set; }
        
        /// <summary>
        /// Filter by parent organization ID
        /// </summary>
        public Guid? ParentId { get; set; }
        
        /// <summary>
        /// Filter by creation date range start
        /// </summary>
        public DateTime? CreatedFrom { get; set; }
        
        /// <summary>
        /// Filter by creation date range end
        /// </summary>
        public DateTime? CreatedTo { get; set; }
    }

    /// <summary>
    /// Data transfer object for setting parent organization
    /// </summary>
    public class SetParentOrganizationDto
    {
        /// <summary>
        /// Parent organization ID
        /// </summary>
        [Required]
        public Guid ParentId { get; set; }
    }

    /// <summary>
    /// Data transfer object for managing organization tags
    /// </summary>
    public class OrganizationTagsDto
    {
        /// <summary>
        /// Tags to add or remove
        /// </summary>
        [Required]
        public List<string> Tags { get; set; }
    }
}
