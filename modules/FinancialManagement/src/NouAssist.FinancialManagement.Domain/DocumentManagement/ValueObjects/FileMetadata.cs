using System;
using System.Collections.Generic;
using System.IO;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.DocumentManagement.ValueObjects
{
    public class FileMetadata : ValueObject
    {
        public string FileName { get; init; }
        public string ContentType { get; init; }
        public long FileSize { get; init; }
        public string FileExtension { get; init; }
        public string FileHash { get; init; }

        private const long MaxFileSize = 52428800; // 50 MB
        private static readonly HashSet<string> AllowedContentTypes = new()
        {
            "application/pdf",
            "image/jpeg",
            "image/png",
            "image/tiff",
            "application/vnd.ms-excel",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "text/csv"
        };

        protected FileMetadata() { }

        public FileMetadata(
            string fileName,
            string contentType,
            long fileSize,
            string fileHash = null)
        {
            FileName = Check.NotNullOrWhiteSpace(fileName, nameof(fileName));
            ContentType = Check.NotNullOrWhiteSpace(contentType, nameof(contentType));
            
            if (fileSize <= 0 || fileSize > MaxFileSize)
            {
                throw new BusinessException("DocumentManagement:InvalidFileSize");
            }

            if (!AllowedContentTypes.Contains(contentType.ToLowerInvariant()))
            {
                throw new BusinessException("DocumentManagement:UnsupportedFileType")
                    .WithData("ContentType", contentType);
            }

            FileSize = fileSize;
            FileExtension = Path.GetExtension(fileName).ToLowerInvariant();
            FileHash = fileHash;
        }

        public bool IsPDF => ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase);
        public bool IsImage => ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
        public bool IsSpreadsheet => ContentType.Contains("excel", StringComparison.OrdinalIgnoreCase) ||
                                     ContentType.Contains("spreadsheet", StringComparison.OrdinalIgnoreCase);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FileName;
            yield return ContentType;
            yield return FileSize;
            yield return FileHash ?? string.Empty;
        }
    }
}
