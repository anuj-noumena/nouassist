namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Constants
{
    /// <summary>
    /// Constants for the Workflow Engine module
    /// </summary>
    public static class WorkflowEngineConsts
    {
        /// <summary>
        /// Maximum name length for workflow definitions and templates
        /// </summary>
        public const int MaxNameLength = 128;
        
        /// <summary>
        /// Maximum length for description fields
        /// </summary>
        public const int MaxDescriptionLength = 1000;
        
        /// <summary>
        /// Maximum length for conditions and expressions
        /// </summary>
        public const int MaxExpressionLength = 2000;
        
        /// <summary>
        /// Maximum length for documentation content
        /// </summary>
        public const int MaxDocumentationLength = 10000;
        
        /// <summary>
        /// Maximum length for BPMN diagram XML
        /// </summary>
        public const int MaxBpmnDiagramLength = 100000;
        
        /// <summary>
        /// Default version number for new workflow definitions
        /// </summary>
        public const int DefaultVersionNumber = 1;
    }
}
