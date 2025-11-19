using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.AssetManagement.Resources.ValueObjects
{
    public class FileMetadata : ValueObject
    {
        public string FileName { get; init; }
        public string FileExtension { get; init; }
        public string MimeType { get; init; }
        public long SizeBytes { get; init; }
        public string Checksum { get; init; }
        public Dictionary<string, object> CustomProperties { get; init; }

        public FileMetadata(
            string fileName,
            string fileExtension,
            string mimeType,
            long sizeBytes,
            string checksum = null,
            Dictionary<string, object> customProperties = null)
        {
            FileName = fileName;
            FileExtension = fileExtension;
            MimeType = mimeType;
            SizeBytes = sizeBytes;
            Checksum = checksum;
            CustomProperties = customProperties ?? new Dictionary<string, object>();
        }

        public string GetReadableSize()
        {
            if (SizeBytes < 1024)
            {
                return $"{SizeBytes} B";
            }

            if (SizeBytes < 1024 * 1024)
            {
                return $"{SizeBytes / 1024.0:F1} KB";
            }

            if (SizeBytes < 1024 * 1024 * 1024)
            {
                return $"{SizeBytes / (1024.0 * 1024.0):F1} MB";
            }

            return $"{SizeBytes / (1024.0 * 1024.0 * 1024.0):F1} GB";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FileName;
            yield return FileExtension;
            yield return MimeType;
            yield return SizeBytes;
            yield return Checksum;
        }
    }
}
