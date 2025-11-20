namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services.Interfaces
{
    /// <summary>
    /// Domain service for business model canvas editing operations
    /// </summary>
    public interface ICanvasEditorService
    {
        Task<BusinessModelCanvas> CreateCanvasAsync(
            string title, 
            string description, 
            Guid ownerId, 
            Guid? tenantId, 
            DateTime creationTime);

        Task<CanvasVersion> CreateVersionAsync(
            BusinessModelCanvas canvas, 
            string versionName, 
            string notes, 
            Guid createdBy, 
            DateTime creationTime);

        Task ValidateComponentContentAsync(
            CanvasComponentType componentType, 
            ComponentContent content);

        Task<ValidationResult> AnalyzeCanvasCompletenessAsync(BusinessModelCanvas canvas);

        Task<List<string>> GenerateContentSuggestionsAsync(
            CanvasComponentType componentType, 
            BusinessModelCanvas canvas);
    }
}
