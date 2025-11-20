using System;
using System.IO;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.DocumentManagement.ValueObjects;

namespace NouAssist.BusinessOperations.DocumentManagement.Interfaces.Services
{
    /// <summary>
    /// Interface for document storage service
    /// </summary>
    public interface IDocumentStorageService
    {
        /// <summary>
        /// Uploads a document to storage
        /// </summary>
        Task<StorageReference> UploadAsync(
            Stream content, 
            string filename, 
            string contentType, 
            Guid? tenantId = null);
            
        /// <summary>
        /// Downloads a document from storage
        /// </summary>
        Task<Stream> DownloadAsync(StorageReference storageReference);
        
        /// <summary>
        /// Gets a temporary download URL for a document
        /// </summary>
        Task<string> GetDownloadUrlAsync(
            StorageReference storageReference, 
            TimeSpan expiration);
            
        /// <summary>
        /// Deletes a document from storage
        /// </summary>
        Task DeleteAsync(StorageReference storageReference);
    }
}
