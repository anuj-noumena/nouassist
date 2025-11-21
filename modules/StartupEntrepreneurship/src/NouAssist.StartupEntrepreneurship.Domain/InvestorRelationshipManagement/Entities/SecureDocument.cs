using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities
{
    /// <summary>
    /// Entity representing a secure document in data room
    /// </summary>
    public class SecureDocument : FullAuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public Guid DataRoomId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DocumentType Type { get; private set; }
        public DocumentMetadata Metadata { get; private set; }
        public string StoragePath { get; private set; }
        public bool IsActive { get; private set; }
        
        private readonly List<AccessLog> _accessLogs;
        public IReadOnlyCollection<AccessLog> AccessLogs => _accessLogs.AsReadOnly();

        protected SecureDocument()
        {
            _accessLogs = new List<AccessLog>();
        }

        internal SecureDocument(
            Guid id,
            Guid dataRoomId,
            string name,
            DocumentType type,
            DocumentMetadata metadata,
            string storagePath,
            string description = null,
            Guid? tenantId = null) : base(id)
        {
            DataRoomId = dataRoomId;
            SetName(name);
            Description = description;
            Type = type;
            Metadata = Check.NotNull(metadata, nameof(metadata));
            SetStoragePath(storagePath);
            IsActive = true;
            TenantId = tenantId;
            _accessLogs = new List<AccessLog>();
        }

        public void UpdateName(string name)
        {
            SetName(name);
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void LogAccess(AccessLog accessLog)
        {
            _accessLogs.Add(Check.NotNull(accessLog, nameof(accessLog)));
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: IRMConsts.DueDiligenceDataRoom.MaxDocumentNameLength
            );
        }

        private void SetStoragePath(string storagePath)
        {
            StoragePath = Check.NotNullOrWhiteSpace(
                storagePath,
                nameof(storagePath),
                maxLength: IRMConsts.DueDiligenceDataRoom.MaxDocumentPathLength
            );
        }
    }
}
