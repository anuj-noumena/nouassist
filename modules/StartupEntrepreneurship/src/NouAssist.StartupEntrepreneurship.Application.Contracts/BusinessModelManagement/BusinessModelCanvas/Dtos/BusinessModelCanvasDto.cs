using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos
{
    public class BusinessModelCanvasDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CurrentVersionNumber { get; set; }
        public Guid? CurrentVersionId { get; set; }
        public Guid OwnerId { get; set; }
        public double ValidationProgress { get; set; }
        public List<CanvasVersionDto> Versions { get; set; }
        public List<CanvasComponentDto> Components { get; set; }

        public BusinessModelCanvasDto()
        {
            Versions = new List<CanvasVersionDto>();
            Components = new List<CanvasComponentDto>();
        }
    }
}
