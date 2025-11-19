using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.AssetManagement.Resources.Enums;

namespace NouAssist.AssetManagement.Resources.Storage.Dtos
{
    public class CreateStorageBackendDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public StorageProvider BackendType { get; set; }

        [Required]
        public Dictionary<string, object> Config { get; set; }

        public bool IsDefault { get; set; }
        public long MaxFileSizeBytes { get; set; } = 104857600; // 100MB default
        public List<string> AllowedMimeTypes { get; set; }
    }
}
