using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos
{
    public class CanvasVersionDto : EntityDto<Guid>
    {
        public Guid CanvasId { get; set; }
        public string VersionName { get; set; }
        public int VersionNumber { get; set; }
        public string Notes { get; set; }
        public bool IsCurrentVersion { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
