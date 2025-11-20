using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos
{
    public class CanvasComponentDto : EntityDto<Guid>
    {
        public Guid CanvasId { get; set; }
        public Guid VersionId { get; set; }
        public CanvasComponentType ComponentType { get; set; }
        public ComponentContentDto Content { get; set; }
        public ValidationStateDto ValidationState { get; set; }
        public DateTime LastModified { get; set; }
        public Guid LastModifiedBy { get; set; }
        public List<HypothesisDto> Hypotheses { get; set; }

        public CanvasComponentDto()
        {
            Hypotheses = new List<HypothesisDto>();
        }
    }
}
