namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create investor with duplicate email
    /// </summary>
    public class DuplicateInvestorEmailException : IRMDomainException
    {
        public DuplicateInvestorEmailException(string email)
            : base("IRM:ErrorMessage:DuplicateInvestorEmail")
        {
            WithData("email", email);
        }
    }
}
