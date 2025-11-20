using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.TermSheet.Dtos
{
    public class TermSheetOfferDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public Guid ProspectId { get; set; }
        public string ProspectName { get; set; }
        public string ProspectCompany { get; set; }
        public Guid? FundraisingRoundId { get; set; }
        public string FundraisingRoundName { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public TermSheetStatus Status { get; set; }
        public string StatusName => Status.ToString();
        public TermComparisonDataDto TermData { get; set; }
        public List<TermSheetClauseDto> Clauses { get; set; }
        public List<NegotiationNoteDto> NegotiationNotes { get; set; }
    }

    public class TermComparisonDataDto
    {
        public decimal PreMoneyValuation { get; set; }
        public decimal PostMoneyValuation { get; set; }
        public decimal InvestmentAmount { get; set; }
        public decimal EquityPercentage { get; set; }
        public bool HasLiquidationPreference { get; set; }
        public decimal LiquidationMultiple { get; set; }
        public bool HasParticipationRights { get; set; }
        public bool HasProRataRights { get; set; }
        public bool HasAntiDilutionProtection { get; set; }
        public Dictionary<string, bool> InvestorRights { get; set; }
    }
}
