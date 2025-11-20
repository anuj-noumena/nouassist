namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Enums
{
    /// <summary>
    /// Types of attributes that can be assigned to requirements
    /// </summary>
    public enum AttributeType : byte
    {
        Priority = 1,
        Complexity = 2,
        Business_Value = 3,
        Risk = 4,
        Effort = 5,
        Domain = 6,
        Stakeholder_Impact = 7,
        Technical_Debt = 8,
        Custom = 99
    }
}
