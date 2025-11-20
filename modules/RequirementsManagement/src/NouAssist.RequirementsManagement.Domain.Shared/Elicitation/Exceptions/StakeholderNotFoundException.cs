namespace NouAssist.RequirementsManagement.Elicitation.Exceptions
{
    /// <summary>
    /// Exception thrown when a stakeholder is not found
    /// </summary>
    public class StakeholderNotFoundException : ElicitationDomainException
    {
        public StakeholderNotFoundException(Guid stakeholderId)
            : base("Elicitation:ErrorMessage:StakeholderNotFound")
        {
            WithData("stakeholderId", stakeholderId);
        }
    }
}
