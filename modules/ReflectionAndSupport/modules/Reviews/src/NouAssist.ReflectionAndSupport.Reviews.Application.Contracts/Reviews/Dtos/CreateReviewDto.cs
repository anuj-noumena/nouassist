using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.ReflectionAndSupport.Reviews.Enums;

namespace NouAssist.ReflectionAndSupport.Reviews.Dtos
{
    public class CreateReviewDto
    {
        [Required]
        public ReviewType ReviewType { get; set; }

        public DateTime? PeriodStart { get; set; }
        public DateTime? PeriodEnd { get; set; }
    }
}
