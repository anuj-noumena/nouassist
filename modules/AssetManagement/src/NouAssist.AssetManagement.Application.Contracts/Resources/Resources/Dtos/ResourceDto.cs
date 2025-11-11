using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.AssetManagement.Resources.Resources.Dtos
{
    public class ResourceDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Kind { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MimeType { get; set; }
        public long? SizeBytes { get; set; }
        public string ReadableSize { get; set; }
        public string Checksum { get; set; }
        public Guid? CurrentVersionId { get; set; }
        public Guid? StorageBackendId { get; set; }
        public string Visibility { get; set; }
        public List<ResourceVersionDto> Versions { get; set; }
        public List<ResourceAclDto> AccessControlList { get; set; }
    }

    public class ResourceVersionDto
    {
        public int VersionNumber { get; set; }
        public string StorageKey { get; set; }
        public string MimeType { get; set; }
        public long SizeBytes { get; set; }
        public string Checksum { get; set; }
        public Guid? UploadedBy { get; set; }
        public DateTime UploadedAt { get; set; }
        public bool IsCurrent { get; set; }
        public string OcrStatus { get; set; }
        public string PreviewStorageKey { get; set; }
    }

    public class ResourceAclDto
    {
        public Guid GrantedToUserId { get; set; }
        public string Permission { get; set; }
        public Guid? GrantedBy { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; }
    }
}
