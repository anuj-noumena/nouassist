using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Projects.Dependencies.Dtos
{
    public class UpdateDependencyDto
    {
        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(5000)]
        public string Notes { get; set; }

        [Range(0, int.MaxValue)]
        public int LagDays { get; set; }

        public bool IsBlocking { get; set; }

        public DateTime? ExpectedResolutionDate { get; set; }
    }
}
