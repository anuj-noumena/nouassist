using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.DocumentManagement.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.DocumentManagement.ValueObjects
{
    /// <summary>
    /// Represents the metadata of a document
    /// </summary>
    public class DocumentMetadata : ValueObject
    {
        public string Filename { get; private set; }
        public string MimeType { get; private set; }
        public long FileSize { get; private set; }
        public Guid UploadedByUserId { get; private set; }
        public DateTime UploadedAt { get; private set; }

        private DocumentMetadata() { }

        public DocumentMetadata(
            string filename,
            string mimeType,
            long fileSize,
            Guid uploadedByUserId,
            DateTime uploadedAt)
        {
            SetFilename(filename);
            SetMimeType(mimeType);
            SetFileSize(fileSize);
            UploadedByUserId = uploadedByUserId;
            UploadedAt = uploadedAt;
        }

        private void SetFilename(string filename)
        {
            Filename = Check.NotNullOrWhiteSpace(
                filename,
                nameof(filename),
                maxLength: DocumentManagementConsts.Document.MaxFilenameLength
            );
        }

        private void SetMimeType(string mimeType)
        {
            MimeType = Check.NotNullOrWhiteSpace(
                mimeType,
                nameof(mimeType),
                maxLength: DocumentManagementConsts.Document.MaxMimeTypeLength
            );
        }

        private void SetFileSize(long fileSize)
        {
            if (fileSize <= 0)
            {
                throw new ArgumentException("File size must be greater than zero", nameof(fileSize));
            }

            if (fileSize > DocumentManagementConsts.Document.MaxFileSizeBytes)
            {
                throw new BusinessException("DocumentManagement:ErrorMessage:FileTooLarge")
                    .WithData("maxSize", DocumentManagementConsts.Document.MaxFileSizeBytes);
            }

            FileSize = fileSize;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Filename;
            yield return MimeType;
            yield return FileSize;
            yield return UploadedByUserId;
            yield return UploadedAt;
        }
    }
}
