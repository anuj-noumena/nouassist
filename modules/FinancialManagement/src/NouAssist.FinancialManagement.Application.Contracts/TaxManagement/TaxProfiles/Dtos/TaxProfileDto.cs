using System;
using System.Collections.Generic;
using NouAssist.TaxManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.TaxManagement.TaxProfiles.Dtos
{
    public class TaxProfileDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public TaxFilingStatus DefaultFilingStatus { get; set; }
        public int NumberOfDependents { get; set; }
        public bool IsItemizer { get; set; }
        public bool IsSelfEmployed { get; set; }
        public string TaxPreparer { get; set; }
        public Dictionary<string, string> TaxSettings { get; set; }
    }

    public class CreateOrUpdateTaxProfileDto
    {
        public TaxFilingStatus DefaultFilingStatus { get; set; }
        public int NumberOfDependents { get; set; }
        public bool IsItemizer { get; set; }
        public bool IsSelfEmployed { get; set; }
        public string TaxPreparer { get; set; }
    }
}
