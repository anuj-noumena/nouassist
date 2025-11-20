namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums
{
    /// <summary>
    /// Represents the trigger type for an automation workflow
    /// </summary>
    public enum AutomationTriggerType
    {
        LeadCreated = 0,
        LeadStatusChanged = 1,
        ContactCreated = 2,
        InteractionLogged = 3,
        ScheduledTime = 4,
        CustomField = 5
    }
}
