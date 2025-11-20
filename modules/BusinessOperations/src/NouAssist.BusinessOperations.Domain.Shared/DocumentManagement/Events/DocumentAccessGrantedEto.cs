using System;
using NouAssist.BusinessOperations.DocumentManagement.Enums;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.DocumentManagement.Events
{
    /// <summary>
    /// Event raised when access is granted to a document
    /// </summary>
    [EventName("NouAssist.BusinessOperations.DocumentManagement.DocumentAccessGranted")]
    public class DocumentAccessGrantedEto
    {
        public Guid DocumentId { get; set; }
        public Guid PrincipalId { get; set; }
        public bool IsRole { get; set; }
        public AccessPermissionLevel PermissionLevel { get; set; }
        public Guid GrantedBy { get; set; }
        public DateTime GrantedAt { get; set; }
        public Guid? TenantId { get; set; }
    }
}
