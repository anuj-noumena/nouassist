using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;

namespace NouAssist.BusinessOperations.FreelanceOperations.Projects.Dtos
{
    public class CreateFreelanceProjectDto
    {
        [Required]
        [StringLength(FreelanceOperationsConsts.Project.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public BillingType BillingType { get; set; }

        // Hourly billing fields
        public decimal? HourlyRate { get; set; }
        public string HourlyRateCurrency { get; set; }

        // Fixed price billing fields
        public decimal? FixedPrice { get; set; }
        public string FixedPriceCurrency { get; set; }

        [Required]
        [StringLength(FreelanceOperationsConsts.Project.MaxDescriptionLength)]
        public string ScopeDescription { get; set; }

        public List<string> ScopeDeliverables { get; set; }
        public bool ScopeIsFlexible { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
