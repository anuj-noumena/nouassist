namespace NouAssist.BusinessOperations.DocumentManagement.Configuration
{
    /// <summary>
    /// Options for document management module
    /// </summary>
    public class DocumentManagementOptions
    {
        /// <summary>
        /// Storage provider name
        /// </summary>
        public string StorageProvider { get; set; } = "LocalStorage";
        
        /// <summary>
        /// Maximum file size in bytes
        /// </summary>
        public long MaxFileSizeBytes { get; set; } = Constants.DocumentManagementConsts.Document.MaxFileSizeBytes;
        
        /// <summary>
        /// Allowed MIME types (empty means all)
        /// </summary>
        public List<string> AllowedMimeTypes { get; set; } = new List<string>();
        
        /// <summary>
        /// Default expiration time for download URLs
        /// </summary>
        public TimeSpan DefaultUrlExpiration { get; set; } = TimeSpan.FromHours(1);
    }
}
