namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Enums
{
    /// <summary>
    /// Available prioritization methods
    /// </summary>
    public enum PrioritizationMethod : byte
    {
        MoSCoW = 1,
        WSJF = 2,
        Kano = 3,
        Numeric_Scoring = 4,
        Stack_Ranking = 5,
        Buy_a_Feature = 6,
        Cost_of_Delay = 7
    }
}
