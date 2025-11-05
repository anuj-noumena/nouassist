using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Resources.Inventory.Dtos
{
    public class ScheduleMaintenanceDto
    {
        [Required]
        public DateTime MaintenanceDate { get; set; }

        public string Notes { get; set; }
    }
}
