namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Services
{
    /// <summary>
    /// Service for generating legal documents from templates
    /// </summary>
    public interface IDocumentGenerationService
    {
        /// <summary>
        /// Creates a new document template
        /// </summary>
        Task<Guid> CreateTemplateAsync(
            string name,
            string content,
            string format,
            Dictionary<string, string> placeholders);
        
        /// <summary>
        /// Updates an existing document template
        /// </summary>
        Task UpdateTemplateAsync(
            Guid id,
            string name,
            string content,
            string format,
            Dictionary<string, string> placeholders);
        
        /// <summary>
        /// Generates a document from a template with provided values
        /// </summary>
        Task<string> GenerateDocumentAsync(
            Guid templateId,
            Dictionary<string, string> values);
        
        /// <summary>
        /// Generates a legal agreement document
        /// </summary>
        Task<string> GenerateAgreementDocumentAsync(
            Guid agreementId,
            Guid templateId,
            Dictionary<string, string> additionalValues = null);
        
        /// <summary>
        /// Generates a corporate record document
        /// </summary>
        Task<string> GenerateCorporateRecordDocumentAsync(
            Guid recordId,
            Guid templateId,
            Dictionary<string, string> additionalValues = null);
    }
}
