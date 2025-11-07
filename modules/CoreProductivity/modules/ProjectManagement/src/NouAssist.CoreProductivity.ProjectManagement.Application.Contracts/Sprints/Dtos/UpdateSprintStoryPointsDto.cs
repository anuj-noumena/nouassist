using System.ComponentModel.DataAnnotations;

namespace NouAssist.CoreProductivity.ProjectManagement.Sprints.Dtos
{
    public class UpdateSprintStoryPointsDto
    {
        [Range(0, int.MaxValue)]
        public int? PlannedStoryPoints { get; set; }

        [Range(0, int.MaxValue)]
        public int? CompletedStoryPoints { get; set; }
    }
}
