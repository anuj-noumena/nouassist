namespace NouAssist.RequirementsManagement.Elicitation.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a stakeholder with an existing email
    /// </summary>
    public class DuplicateStakeholderEmailException : ElicitationDomainException
    {
        public DuplicateStakeholderEmailException(string email)
            : base("Elicitation:ErrorMessage:DuplicateStakeholderEmail")
        {
            WithData("email", email);
        }
    }
}
