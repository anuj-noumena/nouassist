using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;

namespace NouAssist.BusinessOperations.FreelanceOperations.Projects.Dtos
{
    public class UpdateFreelanceProjectDto
    {
        [Required]
        [StringLength(FreelanceOperationsConsts.Project.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(FreelanceOperationsConsts.Project.MaxDescriptionLength)]
        public string ScopeDescription { get; set; }

        public List<string> ScopeDeliverables { get; set; }
        public bool ScopeIsFlexible { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Billing-specific update
        public decimal? HourlyRate { get; set; }
        public string HourlyRateCurrency { get; set; }
        public decimal? FixedPrice { get; set; }
        public string FixedPriceCurrency { get; set; }
    }
}
