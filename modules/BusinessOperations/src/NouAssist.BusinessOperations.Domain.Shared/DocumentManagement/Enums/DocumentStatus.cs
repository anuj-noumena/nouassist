namespace NouAssist.BusinessOperations.DocumentManagement.Enums
{
    /// <summary>
    /// Defines the status of a document
    /// </summary>
    public enum DocumentStatus
    {
        Draft = 0,
        Active = 10,
        Archived = 20,
        MarkedForDeletion = 30,
        Deleted = 40
    }
}
