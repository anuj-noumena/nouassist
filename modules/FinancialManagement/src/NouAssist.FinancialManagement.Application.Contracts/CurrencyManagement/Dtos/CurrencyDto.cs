using System;
using NouAssist.CurrencyManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CurrencyManagement.Dtos
{
    public class CurrencyDto : FullAuditedEntityDto<Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int DecimalPlaces { get; set; }
        public CurrencyStatus Status { get; set; }
        public bool IsBaseCurrency { get; set; }
        public int DisplayOrder { get; set; }
    }
}
