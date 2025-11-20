namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums
{
    /// <summary>
    /// Represents the action type for an automation workflow
    /// </summary>
    public enum AutomationActionType
    {
        SendEmail = 0,
        CreateTask = 1,
        UpdateRecord = 2,
        NotifyUser = 3,
        ExecuteIntegration = 4,
        AssignToUser = 5
    }
}
