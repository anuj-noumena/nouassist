using System.ComponentModel.DataAnnotations;

namespace NouAssist.Projects.SuccessCriteria.Dtos
{
    public class UpdateActualValueDto
    {
        [StringLength(500)]
        public string ActualValue { get; set; }

        public double? ActualNumeric { get; set; }
    }
}
