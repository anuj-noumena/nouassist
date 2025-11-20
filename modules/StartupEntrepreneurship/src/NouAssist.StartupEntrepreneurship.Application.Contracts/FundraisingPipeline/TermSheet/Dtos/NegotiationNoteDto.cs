using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.TermSheet.Dtos
{
    public class NegotiationNoteDto : EntityDto<Guid>
    {
        public Guid TermSheetId { get; set; }
        public Guid? RelatedClauseId { get; set; }
        public string RelatedClauseName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
