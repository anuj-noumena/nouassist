using System;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Prospect.Dtos
{
    public class InteractionLogDto : EntityDto<Guid>
    {
        public Guid ProspectId { get; set; }
        public InteractionType Type { get; set; }
        public string TypeName => Type.ToString();
        public string Summary { get; set; }
        public string Notes { get; set; }
        public DateTime InteractionDate { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
