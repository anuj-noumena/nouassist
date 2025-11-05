using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Resources.Storage.Dtos
{
    public class UpdateStorageBackendDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public Dictionary<string, object> Config { get; set; }
        public bool IsActive { get; set; }
        public long MaxFileSizeBytes { get; set; }
        public List<string> AllowedMimeTypes { get; set; }
    }
}
