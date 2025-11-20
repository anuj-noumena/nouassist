using System.IO;

namespace NouAssist.BusinessOperations.DocumentManagement.Documents
{
    /// <summary>
    /// Contains file content for upload operations
    /// </summary>
    public class FileInputContent
    {
        /// <summary>
        /// File content stream
        /// </summary>
        public Stream Content { get; }
        
        /// <summary>
        /// File size
        /// </summary>
        public long Length { get; }
        
        public FileInputContent(Stream content, long length)
        {
            Content = content;
            Length = length;
        }
    }
}
