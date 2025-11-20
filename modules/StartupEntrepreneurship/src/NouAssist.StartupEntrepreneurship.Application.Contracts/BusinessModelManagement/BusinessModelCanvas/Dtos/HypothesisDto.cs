using System;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos
{
    public class HypothesisDto : EntityDto<Guid>
    {
        public Guid CanvasId { get; set; }
        public Guid ComponentId { get; set; }
        public CanvasComponentType ComponentType { get; set; }
        public HypothesisDetailsDto Details { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
