using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.DocumentManagement.Documents.Dtos
{
    /// <summary>
    /// DTO for revoking access to a document
    /// </summary>
    public class RevokeDocumentAccessDto
    {
        /// <summary>
        /// ID of the document
        /// </summary>
        [Required]
        public Guid DocumentId { get; set; }
        
        /// <summary>
        /// ID of the user or role to revoke access from
        /// </summary>
        [Required]
        public Guid PrincipalId { get; set; }
        
        /// <summary>
        /// Whether the principal is a role
        /// </summary>
        public bool IsRole { get; set; }
    }
}
