using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.TermSheet.Dtos
{
    public class TermSheetClauseDto : EntityDto<Guid>
    {
        public Guid TermSheetId { get; set; }
        public string Name { get; set; }
        public ClauseTextDto Text { get; set; }
        public string Category { get; set; }
        public int Order { get; set; }
        public string Notes { get; set; }
    }

    public class ClauseTextDto
    {
        public string Content { get; set; }
        public bool IsNegotiable { get; set; }
        public string Category { get; set; }
        public string FormattedHtml { get; set; }
    }
}
