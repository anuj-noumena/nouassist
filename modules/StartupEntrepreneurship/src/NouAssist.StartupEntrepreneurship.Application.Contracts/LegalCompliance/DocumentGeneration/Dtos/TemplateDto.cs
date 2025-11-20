using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.DocumentGeneration.Dtos
{
    public class TemplateDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Format { get; set; }
        public Dictionary<string, string> Placeholders { get; set; } = new Dictionary<string, string>();
        public DateTime LastUpdated { get; set; }
    }
