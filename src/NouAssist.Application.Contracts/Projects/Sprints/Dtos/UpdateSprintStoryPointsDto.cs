using System.ComponentModel.DataAnnotations;

namespace NouAssist.Projects.Sprints.Dtos
{
    public class UpdateSprintStoryPointsDto
    {
        [Range(0, int.MaxValue)]
        public int? PlannedStoryPoints { get; set; }

        [Range(0, int.MaxValue)]
        public int? CompletedStoryPoints { get; set; }
    }
}
