namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects
{
    /// <summary>
    /// Value object representing contact information for an investor
    /// </summary>
    public class ContactDetails : ValueObject
    {
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string LinkedIn { get; private set; }
        public string Website { get; private set; }
        public string Address { get; private set; }

        private ContactDetails() { }

        public ContactDetails(
            string email,
            string phone = null,
            string linkedIn = null,
            string website = null,
            string address = null)
        {
            SetEmail(email);
            SetPhone(phone);
            SetLinkedIn(linkedIn);
            SetWebsite(website);
            Address = address;
        }

        private void SetEmail(string email)
        {
            if (!email.IsNullOrWhiteSpace() && !IsValidEmail(email))
            {
                throw new BusinessException("IRM:Validation:InvalidEmail");
            }

            Email = Check.NotNullOrWhiteSpace(
                email,
                nameof(email),
                maxLength: IRMConsts.InvestorProfile.MaxEmailLength
            );
        }

        private void SetPhone(string phone)
        {
            Phone = Check.Length(phone, nameof(phone), IRMConsts.InvestorProfile.MaxPhoneLength);
        }

        private void SetLinkedIn(string linkedIn)
        {
            if (!linkedIn.IsNullOrWhiteSpace() && !IsValidUrl(linkedIn))
            {
                throw new BusinessException("IRM:Validation:InvalidUrl");
            }

            LinkedIn = Check.Length(linkedIn, nameof(linkedIn), IRMConsts.InvestorProfile.MaxLinkedInLength);
        }

        private void SetWebsite(string website)
        {
            if (!website.IsNullOrWhiteSpace() && !IsValidUrl(website))
            {
                throw new BusinessException("IRM:Validation:InvalidUrl");
            }

            Website = Check.Length(website, nameof(website), IRMConsts.InvestorProfile.MaxWebsiteLength);
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Email;
            yield return Phone;
            yield return LinkedIn;
            yield return Website;
            yield return Address;
        }
    }
}
