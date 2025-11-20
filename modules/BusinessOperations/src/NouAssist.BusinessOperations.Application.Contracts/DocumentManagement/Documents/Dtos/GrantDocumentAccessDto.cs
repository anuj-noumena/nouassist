using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.DocumentManagement.Enums;

namespace NouAssist.BusinessOperations.DocumentManagement.Documents.Dtos
{
    /// <summary>
    /// DTO for granting access to a document
    /// </summary>
    public class GrantDocumentAccessDto
    {
        /// <summary>
        /// ID of the document
        /// </summary>
        [Required]
        public Guid DocumentId { get; set; }
        
        /// <summary>
        /// ID of the user or role to grant access to
        /// </summary>
        [Required]
        public Guid PrincipalId { get; set; }
        
        /// <summary>
        /// Whether the principal is a role
        /// </summary>
        public bool IsRole { get; set; }
        
        /// <summary>
        /// Permission level to grant
        /// </summary>
        [Required]
        public AccessPermissionLevel PermissionLevel { get; set; }
    }
}
