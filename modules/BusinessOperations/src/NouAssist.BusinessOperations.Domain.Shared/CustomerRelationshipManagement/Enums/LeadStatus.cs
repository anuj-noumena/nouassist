namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums
{
    /// <summary>
    /// Represents the status of a lead in the sales pipeline
    /// </summary>
    public enum LeadStatus
    {
        New = 0,
        Contacted = 1,
        Qualified = 2,
        Unqualified = 3,
        Converted = 4,
        Archived = 5
    }
}
