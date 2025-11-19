using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IdentityAccess.Users.Aggregates
{
    public class User : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? Avatar { get; set; }
        public string? Email { get; private set; }
        public string? Timezone { get; set; }
        public string? Locale { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; private set; }

        protected User() { }

        public User(Guid id, string? email = null, Guid? tenantId = null) : base(id)
        {
            Email = email;
            TenantId = tenantId;
            IsActive = true;
        }

        public string GetFullName()
        {
            if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName))
            {
                return "Unknown User";
            }

            return $"{FirstName} {LastName}".Trim();
        }

        public string GetInitials()
        {
            var first = !string.IsNullOrWhiteSpace(FirstName) ? FirstName[0].ToString() : "";
            var last = !string.IsNullOrWhiteSpace(LastName) ? LastName[0].ToString() : "";
            return $"{first}{last}".ToUpper();
        }

        public string GetDisplayName() => 
            GetFullName() != "Unknown User" ? GetFullName() : Email ?? Id.ToString();

        public void UpdateProfile(string? firstName, string? lastName, string? bio, string? avatar)
        {
            FirstName = firstName;
            LastName = lastName;
            Bio = Check.Length(bio, nameof(bio), 500);
            Avatar = avatar;
        }

        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
