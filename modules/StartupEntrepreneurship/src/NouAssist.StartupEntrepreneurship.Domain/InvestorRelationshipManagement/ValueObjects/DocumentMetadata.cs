using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects
{
    /// <summary>
    /// Value object representing metadata for documents in data room
    /// </summary>
    public class DocumentMetadata : ValueObject
    {
        public string FileName { get; private set; }
        public string MimeType { get; private set; }
        public long FileSize { get; private set; }
        public string Hash { get; private set; }
        public DateTime UploadedAt { get; private set; }
        public Guid UploadedBy { get; private set; }

        private DocumentMetadata() { }

        public DocumentMetadata(
            string fileName,
            string mimeType,
            long fileSize,
            string hash,
            DateTime uploadedAt,
            Guid uploadedBy)
        {
            FileName = Check.NotNullOrWhiteSpace(
                fileName,
                nameof(fileName),
                maxLength: IRMConsts.DueDiligenceDataRoom.MaxDocumentNameLength
            );
            MimeType = Check.NotNullOrWhiteSpace(mimeType, nameof(mimeType));
            FileSize = fileSize > 0 ? fileSize : throw new BusinessException("IRM:ErrorMessage:InvalidOperation", "File size must be greater than 0");
            Hash = Check.NotNullOrWhiteSpace(hash, nameof(hash));
            UploadedAt = uploadedAt;
            UploadedBy = uploadedBy;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FileName;
            yield return MimeType;
            yield return FileSize;
            yield return Hash;
            yield return UploadedAt;
            yield return UploadedBy;
        }
    }
}
