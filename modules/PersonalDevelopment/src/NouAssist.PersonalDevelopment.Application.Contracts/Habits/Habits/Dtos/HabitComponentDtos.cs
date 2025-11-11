using System.ComponentModel.DataAnnotations;
using NouAssist.PersonalDevelopment.Habits.Enums;

namespace NouAssist.PersonalDevelopment.Habits.Habits.Dtos
{
    public class AddHabitCueDto
    {
        [Required]
        public CueType CueType { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(200)]
        public string CueValue { get; set; }
    }

    public class AddHabitResponseDto
    {
        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(2000)]
        public string Details { get; set; }

        public int? DurationMinutes { get; set; }
    }

    public class AddHabitRewardDto
    {
        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(500)]
        public string Craving { get; set; }
    }

    public class AddHabitStrategyDto
    {
        [Required]
        public HabitStrategyType StrategyType { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
    }
}
