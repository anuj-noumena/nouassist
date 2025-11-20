using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel.Dtos
{
    public class RevenueScenarioDto : EntityDto<Guid>
    {
        public Guid RevenueModelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ScenarioParametersDto Parameters { get; set; }
        public List<ProjectedRevenueDataDto> Projections { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? LastSimulatedAt { get; set; }

        public RevenueScenarioDto()
        {
            Projections = new List<ProjectedRevenueDataDto>();
        }
    }
}
