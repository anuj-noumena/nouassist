using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using NouAssist.AssetManagement.Resources.Enums;

namespace NouAssist.AssetManagement.Resources.Resources.Dtos
{
    public class UploadResourceDto
    {
        [Required]
        public IFormFile File { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public ResourceKind? Kind { get; set; }
        public string Visibility { get; set; } = "private";
    }
}
