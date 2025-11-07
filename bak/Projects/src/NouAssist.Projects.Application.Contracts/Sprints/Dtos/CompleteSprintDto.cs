using System.ComponentModel.DataAnnotations;

namespace NouAssist.Projects.Sprints.Dtos
{
    public class CompleteSprintDto
    {
        [StringLength(5000)]
        public string RetrospectiveNotes { get; set; }
    }
}
