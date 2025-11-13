namespace NouAssist.WorkManagement.ProjectManagement.Enums
{
    public enum SuccessCriterionType : byte
    {
        Quantitative = 1,  // Measurable with numbers
        Qualitative = 2,   // Subjective assessment
        Binary = 3,        // Yes/No
        Milestone = 4      // Specific event
    }
    
    public enum CriterionCriticality
    {
        MustHave = 1,    // Weight 8-10
        ShouldHave = 2,  // Weight 5-7
        NiceToHave = 3   // Weight 1-4
    }
}
