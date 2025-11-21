using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Events;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Aggregates
{
    /// <summary>
    /// Aggregate root representing a due diligence data room with secure documents and access control
    /// </summary>
    public class DueDiligenceDataRoom : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? ExpiryDate { get; private set; }
        public bool RequirePasswordAccess { get; private set; }
        public string AccessPassword { get; private set; }

        // Collections within aggregate boundary
        private readonly List<SecureDocument> _documents;
        private readonly Dictionary<Guid, AccessLevel> _investorAccess;

        public IReadOnlyCollection<SecureDocument> Documents => _documents.AsReadOnly();
        public IReadOnlyDictionary<Guid, AccessLevel> InvestorAccess => _investorAccess.AsReadOnly();

        protected DueDiligenceDataRoom()
        {
            _documents = new List<SecureDocument>();
            _investorAccess = new Dictionary<Guid, AccessLevel>();
        }

        public static DueDiligenceDataRoom Create(
            Guid id,
            string name,
            string description = null,
            DateTime? expiryDate = null,
            bool requirePasswordAccess = false,
            string accessPassword = null,
            Guid? tenantId = null)
        {
            return new DueDiligenceDataRoom(id, name, description, expiryDate, requirePasswordAccess, accessPassword, tenantId);
        }

        private DueDiligenceDataRoom(
            Guid id,
            string name,
            string description = null,
            DateTime? expiryDate = null,
            bool requirePasswordAccess = false,
            string accessPassword = null,
            Guid? tenantId = null) : base(id)
        {
            SetName(name);
            Description = Check.Length(description, nameof(description), IRMConsts.DueDiligenceDataRoom.MaxDescriptionLength);
            IsActive = true;
            ExpiryDate = expiryDate;
            RequirePasswordAccess = requirePasswordAccess;
            AccessPassword = requirePasswordAccess ? Check.NotNullOrWhiteSpace(accessPassword, nameof(accessPassword)) : null;
            TenantId = tenantId;
            _documents = new List<SecureDocument>();
            _investorAccess = new Dictionary<Guid, AccessLevel>();
        }

        public void UpdateName(string name)
        {
            SetName(name);
        }

        public void UpdateDescription(string description)
        {
            Description = Check.Length(description, nameof(description), IRMConsts.DueDiligenceDataRoom.MaxDescriptionLength);
        }

        public void SetExpiryDate(DateTime? expiryDate)
        {
            if (expiryDate.HasValue && expiryDate.Value <= DateTime.UtcNow)
                throw new BusinessException("IRM:ErrorMessage:InvalidOperation", "Expiry date must be in the future");

            ExpiryDate = expiryDate;
        }

        public void SetPasswordAccess(bool require, string password = null)
        {
            RequirePasswordAccess = require;
            if (require)
            {
                AccessPassword = Check.NotNullOrWhiteSpace(password, nameof(password));
            }
            else
            {
                AccessPassword = null;
            }
        }

        public void GrantAccess(Guid investorId, AccessLevel accessLevel)
        {
            _investorAccess[investorId] = accessLevel;

            AddDistributedEvent(new DueDiligenceAccessGrantedEto
            {
                Id = GuidGenerator.Create(),
                InvestorId = investorId,
                DataRoomId = Id,
                AccessLevel = accessLevel,
                ExpiryDate = ExpiryDate ?? DateTime.MaxValue,
                TenantId = TenantId
            });
        }

        public void RevokeAccess(Guid investorId)
        {
            _investorAccess.Remove(investorId);
        }

        public AccessLevel? GetInvestorAccessLevel(Guid investorId)
        {
            return _investorAccess.TryGetValue(investorId, out var accessLevel) ? accessLevel : null;
        }

        public bool CanInvestorAccess(Guid investorId)
        {
            if (!IsActive)
                return false;

            if (ExpiryDate.HasValue && ExpiryDate.Value <= DateTime.UtcNow)
                return false;

            return _investorAccess.ContainsKey(investorId);
        }

        public void AddDocument(
            string name,
            DocumentType type,
            DocumentMetadata metadata,
            string storagePath,
            string description = null)
        {
            var document = new SecureDocument(
                GuidGenerator.Create(),
                Id,
                name,
                type,
                metadata,
                storagePath,
                description,
                TenantId
            );

            _documents.Add(document);
        }

        public void UpdateDocument(Guid documentId, string name = null, string description = null)
        {
            var document = _documents.FirstOrDefault(d => d.Id == documentId);
            if (document == null)
                throw new BusinessException("IRM:ErrorMessage:EntityNotFound", "Document not found");

            if (name != null)
                document.UpdateName(name);
            
            if (description != null)
                document.UpdateDescription(description);
        }

        public void RemoveDocument(Guid documentId)
        {
            var document = _documents.FirstOrDefault(d => d.Id == documentId);
            if (document != null)
            {
                document.Deactivate();
            }
        }

        public void LogDocumentAccess(Guid documentId, AccessLog accessLog)
        {
            var document = _documents.FirstOrDefault(d => d.Id == documentId);
            if (document == null)
                throw new BusinessException("IRM:ErrorMessage:EntityNotFound", "Document not found");

            document.LogAccess(accessLog);
        }

        public bool CanInvestorAccessDocument(Guid investorId, Guid documentId)
        {
            if (!CanInvestorAccess(investorId))
                return false;

            var document = _documents.FirstOrDefault(d => d.Id == documentId);
            if (document == null || !document.IsActive)
                return false;

            var accessLevel = GetInvestorAccessLevel(investorId);
            return accessLevel.HasValue && accessLevel.Value != AccessLevel.Restricted;
        }

        public List<SecureDocument> GetAccessibleDocuments(Guid investorId)
        {
            if (!CanInvestorAccess(investorId))
                return new List<SecureDocument>();

            var accessLevel = GetInvestorAccessLevel(investorId);
            if (!accessLevel.HasValue || accessLevel.Value == AccessLevel.Restricted)
                return new List<SecureDocument>();

            return _documents.Where(d => d.IsActive).ToList();
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: IRMConsts.DueDiligenceDataRoom.MaxNameLength
            );
        }
    }
}
