namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Enums
{
    /// <summary>
    /// Types of relationships between requirements
    /// </summary>
    public enum RelationshipType : byte
    {
        Dependency = 1,
        Conflict = 2,
        Parent_Child = 3,
        Similar = 4,
        Alternative = 5,
        Derives_From = 6,
        Supports = 7,
        Constrains = 8
    }
}
