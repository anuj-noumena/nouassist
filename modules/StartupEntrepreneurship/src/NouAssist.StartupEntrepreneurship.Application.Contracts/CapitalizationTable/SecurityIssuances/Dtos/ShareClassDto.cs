using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityIssuances.Dtos
{
    /// <summary>
    /// DTO for ShareClass information
    /// </summary>
    public class ShareClassDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SecurityType { get; set; }
        public decimal ParValue { get; set; }
        public int VotingRights { get; set; }
        public bool HasDividendRights { get; set; }
        public bool HasLiquidationPreference { get; set; }
        public decimal LiquidationMultiple { get; set; }
        public bool HasAntiDilutionRights { get; set; }
        public string AntiDilutionType { get; set; }
    }
}
