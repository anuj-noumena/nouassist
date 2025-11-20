using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.TermSheet.Dtos
{
    public class CreateTermSheetClauseDto
    {
        [Required]
        [StringLength(FundraisingPipelineConsts.TermSheet.MaxClauseNameLength)]
        public string Name { get; set; }

        [Required]
        public CreateClauseTextDto Text { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; }

        [Required]
        public int Order { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }
    }

    public class CreateClauseTextDto
    {
        [Required]
        [StringLength(FundraisingPipelineConsts.TermSheet.MaxClauseValueLength)]
        public string Content { get; set; }

        public bool IsNegotiable { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; }

        [StringLength(4000)]
        public string FormattedHtml { get; set; }
    }
}
