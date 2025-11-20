namespace NouAssist.BusinessOperations.FreelanceOperations.ValueObjects
{
    /// <summary>
    /// Value object representing client contact information
    /// </summary>
    public class ClientContactInfo : ValueObject
    {
        /// <summary>
        /// Client's email address
        /// </summary>
        public string Email { get; private set; }
        
        /// <summary>
        /// Client's phone number
        /// </summary>
        public string Phone { get; private set; }
        
        /// <summary>
        /// Client's website
        /// </summary>
        public string Website { get; private set; }
        
        /// <summary>
        /// Client's address
        /// </summary>
        public string Address { get; private set; }

        protected ClientContactInfo()
        {
        }

        public ClientContactInfo(
            string email, 
            string phone = null, 
            string website = null, 
            string address = null)
        {
            // Email is required and must be valid
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new BusinessException("FreelanceOperations:Validation:Required", "Email");
            }
            
            // Simple email validation
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new BusinessException("FreelanceOperations:Validation:InvalidValue", "Email");
            }
            
            Email = email;
            Phone = phone;
            Website = website;
            Address = address;
        }

        /// <summary>
        /// Updates the phone number
        /// </summary>
        public void UpdatePhone(string phone)
        {
            Phone = phone;
        }
        
        /// <summary>
        /// Updates the website
        /// </summary>
        public void UpdateWebsite(string website)
        {
            Website = website;
        }
        
        /// <summary>
        /// Updates the address
        /// </summary>
        public void UpdateAddress(string address)
        {
            Address = address;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Email;
            yield return Phone ?? string.Empty;
            yield return Website ?? string.Empty;
            yield return Address ?? string.Empty;
        }
    }
}
