namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Constants
{
    /// <summary>
    /// Constants for the Analysis & Prioritization domain
    /// </summary>
    public static class AnalysisConsts
    {
        public const int MaxAttributeNameLength = 100;
        public const int MaxAttributeDescriptionLength = 500;
        public const int MaxGlossaryTermNameLength = 100;
        public const int MaxGlossaryTermDefinitionLength = 1000;
        public const int MaxPrioritizationActivityNameLength = 200;
        public const int MaxPrioritizationActivityDescriptionLength = 1000;
        public const int MaxImpactDescriptionLength = 2000;
        public const int MaxRelationshipDescriptionLength = 500;
        
        public const decimal MinPriorityWeight = 0.0m;
        public const decimal MaxPriorityWeight = 1.0m;
    }
}
