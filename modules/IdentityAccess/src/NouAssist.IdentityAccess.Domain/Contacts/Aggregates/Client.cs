using System;
using System.Collections.Generic;
using NouAssist.IdentityAccess.Contacts.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IdentityAccess.Contacts.Aggregates
{
    public class Client : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Name { get; private set; }
        public string LegalName { get; set; }
        public string Industry { get; set; }
        public string Website { get; set; }
        public Guid? PrimaryContactId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public ClientStatus Status { get; private set; }
        public DateTime? ClientSince { get; private set; }
        public decimal? LifetimeValue { get; private set; }
        public decimal? CurrentBalance { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? InactiveSince { get; private set; }
        public DateTime? LastContactedAt { get; private set; }
        public DateTime? NextReviewDate { get; set; }

        private readonly List<string> _tags = new();
        public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();

        protected Client() { }

        public Client(
            Guid id,
            Guid ownerId,
            string name,
            Guid? tenantId = null,
            string email = null,
            string phone = null)
            : base(id)
        {
            OwnerId = ownerId;
            SetName(name);
            TenantId = tenantId;
            Email = email;
            Phone = phone;
            Status = ClientStatus.Potential;
            IsActive = true;
            ClientSince = DateTime.UtcNow;
        }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 200);
        }

        public void Activate()
        {
            Status = ClientStatus.Active;
            IsActive = true;
            InactiveSince = null;
        }

        public void Deactivate(string reason = null)
        {
            IsActive = false;
            InactiveSince = DateTime.UtcNow;
        }

        public void PutOnHold(string reason = null)
        {
            Status = ClientStatus.OnHold;
        }

        public void MarkAsLost(string reason = null)
        {
            Status = ClientStatus.Lost;
            IsActive = false;
            InactiveSince = DateTime.UtcNow;
        }

        public void ConvertToPaying()
        {
            if (Status != ClientStatus.Potential)
                throw new BusinessException("Clients:CanOnlyConvertPotential");

            Status = ClientStatus.Active;
            ClientSince = DateTime.UtcNow;
        }

        public void RecordContact()
        {
            LastContactedAt = DateTime.UtcNow;
        }

        public void UpdateBalance(decimal amount)
        {
            CurrentBalance = (CurrentBalance ?? 0) + amount;
        }

        public void AddTag(string tag)
        {
            if (!_tags.Contains(tag))
                _tags.Add(tag);
        }

        public bool NeedsReview => NextReviewDate.HasValue && DateTime.UtcNow > NextReviewDate.Value;
    }
}
