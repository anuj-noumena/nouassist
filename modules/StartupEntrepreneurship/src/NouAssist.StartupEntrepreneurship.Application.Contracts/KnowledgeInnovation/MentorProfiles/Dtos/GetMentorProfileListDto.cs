using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.MentorProfiles.Dtos
{
    public class GetMentorProfileListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public string ExpertiseKeyword { get; set; }
        public int? MinimumExperienceYears { get; set; }
        public string Tag { get; set; }
    }
}
