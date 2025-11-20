using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Prospect.Dtos
{
    public class UpdateProspectStatusDto
    {
        [Required]
        public ProspectStatus Status { get; set; }
    }
}
