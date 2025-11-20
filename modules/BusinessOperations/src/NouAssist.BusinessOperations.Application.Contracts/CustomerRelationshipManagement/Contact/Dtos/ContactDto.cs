using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Lead.Dtos;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Contact.Dtos
{
    /// <summary>
    /// Data transfer object for Contact entity
    /// </summary>
    public class ContactDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// Full name of the contact
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Contact's job title
        /// </summary>
        public string JobTitle { get; set; }
        
        /// <summary>
        /// Contact information
        /// </summary>
        public ContactInfoDto ContactInfo { get; set; }
        
        /// <summary>
        /// Organization this contact belongs to
        /// </summary>
        public Guid? OrganizationId { get; set; }
        
        /// <summary>
        /// Organization name (if available)
        /// </summary>
        public string OrganizationName { get; set; }
        
        /// <summary>
        /// Whether this is a primary contact for the organization
        /// </summary>
        public bool IsPrimaryContact { get; set; }
        
        /// <summary>
        /// Lead this contact was converted from
        /// </summary>
        public Guid? ConvertedFromLeadId { get; set; }
        
        /// <summary>
        /// Notes about this contact
        /// </summary>
        public string Notes { get; set; }
        
        /// <summary>
        /// Whether this contact is active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Tags for categorization
        /// </summary>
        public List<string> Tags { get; set; }
        
        /// <summary>
        /// Group IDs this contact belongs to
        /// </summary>
        public List<Guid> GroupIds { get; set; }
    }

    /// <summary>
    /// Data transfer object for creating a contact
    /// </summary>
    public class CreateContactDto
    {
        /// <summary>
        /// Full name of the contact
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
        /// Contact's job title
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Title)]
        public string JobTitle { get; set; }
        
        /// <summary>
        /// Organization this contact belongs to
        /// </summary>
        public Guid? OrganizationId { get; set; }
        
        /// <summary>
        /// Whether this is a primary contact for the organization
        /// </summary>
        public bool IsPrimaryContact { get; set; }
        
        /// <summary>
        /// Notes about this contact
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Note)]
        public string Notes { get; set; }
        
        /// <summary>
        /// Initial tags (optional)
        /// </summary>
        public List<string> Tags { get; set; }
    }

    /// <summary>
    /// Data transfer object for updating a contact
    /// </summary>
    public class UpdateContactDto
    {
        /// <summary>
        /// Full name of the contact
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Name)]
        public string Name { get; set; }
        
        /// <summary>
        /// Contact information
        /// </summary>
        public UpdateContactInfoDto ContactInfo { get; set; }
        
        /// <summary>
        /// Contact's job title
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Title)]
        public string JobTitle { get; set; }
        
        /// <summary>
        /// Notes about this contact
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Note)]
        public string Notes { get; set; }
    }

    /// <summary>
    /// Data transfer object for contact list filtering and pagination
    /// </summary>
    public class GetContactListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to search in contact name, email, and organization
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by organization ID
        /// </summary>
        public Guid? OrganizationId { get; set; }
        
        /// <summary>
        /// Filter by active status
        /// </summary>
        public bool? IsActive { get; set; }
        
        /// <summary>
        /// Filter by tags (comma-separated)
        /// </summary>
        public string Tags { get; set; }
        
        /// <summary>
        /// Filter by group ID
        /// </summary>
        public Guid? GroupId { get; set; }
        
        /// <summary>
        /// Filter for primary contacts only
        /// </summary>
        public bool? PrimaryOnly { get; set; }
        
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
    /// Data transfer object for assigning a contact to an organization
    /// </summary>
    public class AssignToOrganizationDto
    {
        /// <summary>
        /// Organization ID
        /// </summary>
        [Required]
        public Guid OrganizationId { get; set; }
        
        /// <summary>
        /// Whether this is a primary contact for the organization
        /// </summary>
        public bool IsPrimaryContact { get; set; }
    }

    /// <summary>
    /// Data transfer object for managing contact tags
    /// </summary>
    public class ContactTagsDto
    {
        /// <summary>
        /// Tags to add or remove
        /// </summary>
        [Required]
        public List<string> Tags { get; set; }
    }

    /// <summary>
    /// Data transfer object for managing contact groups
    /// </summary>
    public class ContactGroupsDto
    {
        /// <summary>
        /// Group IDs to add or remove
        /// </summary>
        [Required]
        public List<Guid> GroupIds { get; set; }
    }
}
