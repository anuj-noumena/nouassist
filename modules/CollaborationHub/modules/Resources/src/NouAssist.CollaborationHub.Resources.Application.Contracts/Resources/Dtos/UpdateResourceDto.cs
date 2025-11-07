using System.ComponentModel.DataAnnotations;

namespace NouAssist.CollaborationHub.Resources.Resources.Dtos
{
    public class UpdateResourceDto
    {
        [MaxLength(500)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        public string Visibility { get; set; }
    }
}
