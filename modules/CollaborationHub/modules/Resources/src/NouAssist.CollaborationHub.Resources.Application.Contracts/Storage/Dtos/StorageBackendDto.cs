using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CollaborationHub.Resources.Storage.Dtos
{
    public class StorageBackendDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string BackendType { get; set; }
        public Dictionary<string, object> Config { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public long MaxFileSizeBytes { get; set; }
        public List<string> AllowedMimeTypes { get; set; }
    }
}
