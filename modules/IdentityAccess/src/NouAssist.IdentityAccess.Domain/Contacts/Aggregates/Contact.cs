using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IdentityAccess.Contacts.Enums;
using NouAssist.IdentityAccess.Contacts.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IdentityAccess.Contacts.Aggregates
{
    public class Contact : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        
        // Name
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrganizationName { get; set; }
        public string DisplayName { get; private set; }
        
        // Professional
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public Guid? ClientId { get; set; }
        
        // Contact info
        public string PrimaryEmail { get; private set; }
        public string PrimaryPhone { get; private set; }
        public string Website { get; set; }
        
        // Categorization
        public ContactType ContactType { get; set; }
        public ContactRelationship Relationship { get; set; }
        public double Importance { get; set; }
        public string Notes { get; set; }
        
        // Status
        public bool IsActive { get; private set; }
        public DateTime? ArchivedAt { get; private set; }
        
        // Interaction tracking
        public DateTime? LastContactedAt { get; private set; }
        public DateTime? LastMetAt { get; private set; }
        public int ContactFrequencyDays { get; set; }
        public DateTime? NextFollowupDate { get; set; }

        private readonly List<ContactMethod> _contactMethods = new();
        public IReadOnlyCollection<ContactMethod> ContactMethods => _contactMethods.AsReadOnly();

        private readonly List<string> _tags = new();
        public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();

        private readonly List<ValueObjects.SocialProfile> _socialProfiles = new();
        public IReadOnlyCollection<ValueObjects.SocialProfile> SocialProfiles => _socialProfiles.AsReadOnly();
        
        protected Contact() { }

        public Contact(
            Guid id,
            Guid ownerId,
            Guid? tenantId = null,
            string firstName = null,
            string lastName = null,
            string organizationName = null,
            string email = null,
            string phone = null)
            : base(id)
        {
            OwnerId = ownerId;
            TenantId = tenantId;
            FirstName = firstName;
            LastName = lastName;
            OrganizationName = organizationName;
            IsActive = true;
            ContactFrequencyDays = 7;
            Importance = 0;
            ContactType = string.IsNullOrEmpty(organizationName) ? ContactType.Person : ContactType.Organization;

            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName) && string.IsNullOrEmpty(organizationName))
            {
                throw new BusinessException("Contacts:NameRequired")
                    .WithData("message", "Contact must have a name or organization");
            }

            if (!string.IsNullOrEmpty(email))
            {
                SetPrimaryEmail(email);
                _contactMethods.Add(new ContactMethod("Email", email, "Email", true));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                SetPrimaryPhone(phone);
                _contactMethods.Add(new ContactMethod("Phone", phone, "Phone", true));
            }

            UpdateDisplayName();
        }

        public void SetPrimaryEmail(string email)
        {
            PrimaryEmail = Check.NotNullOrWhiteSpace(email, nameof(email));
            // TODO: Add email validation
        }

        public void SetPrimaryPhone(string phone)
        {
            PrimaryPhone = Check.NotNullOrWhiteSpace(phone, nameof(phone));
        }

        public void UpdateDisplayName()
        {
            if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
            {
                DisplayName = $"{FirstName} {LastName}";
            }
            else if (!string.IsNullOrEmpty(FirstName))
            {
                DisplayName = FirstName;
            }
            else if (!string.IsNullOrEmpty(OrganizationName))
            {
                DisplayName = OrganizationName;
            }
            else
            {
                DisplayName = "Unknown";
            }
        }

        public void RecordContact()
        {
            LastContactedAt = DateTime.UtcNow;
        }

        public void RecordMeeting()
        {
            LastMetAt = DateTime.UtcNow;
            LastContactedAt = DateTime.UtcNow;
        }

        public void ScheduleFollowup(DateTime date)
        {
            NextFollowupDate = date;
        }

        public void Archive()
        {
            IsActive = false;
            ArchivedAt = DateTime.UtcNow;
        }

        public void Unarchive()
        {
            IsActive = true;
            ArchivedAt = null;
        }

        public void AddTag(string tag)
        {
            if (!_tags.Contains(tag))
            {
                _tags.Add(tag);
            }
        }

        public void RemoveTag(string tag)
        {
            _tags.Remove(tag);
        }

        public void AddContactMethod(ContactMethod method)
        {
            _contactMethods.Add(method);
        }

        public bool HasEmail => _contactMethods.Any(m => m.Type.Equals("Email", StringComparison.OrdinalIgnoreCase));
        public bool HasPhone => _contactMethods.Any(m => m.Type.Equals("Phone", StringComparison.OrdinalIgnoreCase));
        public bool NeedsFollowup => NextFollowupDate.HasValue && DateTime.UtcNow > NextFollowupDate.Value;
        
        
        public void AddSocialProfile(ValueObjects.SocialProfile profile)
        {
            if (_socialProfiles.Any(p => p.Platform == profile.Platform && p.Username == profile.Username))
            {
                return; // Already exists
            }

            _socialProfiles.Add(profile);
        }

        public void RemoveSocialProfile(SocialPlatform platform, string username)
        {
            _socialProfiles.RemoveAll(p => p.Platform == platform && p.Username == username);
        }

        public bool HasSocialProfile(SocialPlatform platform) =>
            _socialProfiles.Any(p => p.Platform == platform);

    }
}
