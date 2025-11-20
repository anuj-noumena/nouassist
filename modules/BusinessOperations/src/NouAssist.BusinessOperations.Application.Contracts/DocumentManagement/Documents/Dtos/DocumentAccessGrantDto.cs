using System;
using NouAssist.BusinessOperations.DocumentManagement.Enums;

namespace NouAssist.BusinessOperations.DocumentManagement.Documents.Dtos
{
    /// <summary>
    /// DTO for document access grant information
    /// </summary>
    public class DocumentAccessGrantDto
    {
        /// <summary>
        /// ID of the user or role
        /// </summary>
        public Guid PrincipalId { get; set; }
        
        /// <summary>
        /// Whether the principal is a role
        /// </summary>
        public bool IsRole { get; set; }
        
        /// <summary>
        /// Permission level granted
        /// </summary>
        public AccessPermissionLevel PermissionLevel { get; set; }
        
        /// <summary>
        /// ID of the user who granted the access
        /// </summary>
        public Guid GrantedBy { get; set; }
        
        /// <summary>
        /// When the access was granted
        /// </summary>
        public DateTime GrantedAt { get; set; }
    }
}
