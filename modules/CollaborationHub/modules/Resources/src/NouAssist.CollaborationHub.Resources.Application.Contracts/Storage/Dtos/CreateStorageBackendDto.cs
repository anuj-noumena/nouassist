using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.CollaborationHub.Resources.Enums;

namespace NouAssist.CollaborationHub.Resources.Storage.Dtos
{
    public class CreateStorageBackendDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public StorageBackendType BackendType { get; set; }

        [Required]
        public Dictionary<string, object> Config { get; set; }

        public bool IsDefault { get; set; }
        public long MaxFileSizeBytes { get; set; } = 104857600; // 100MB default
        public List<string> AllowedMimeTypes { get; set; }
    }
}
