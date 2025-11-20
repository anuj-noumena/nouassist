using System;

namespace NouAssist.LearningDevelopment.Integration.LearningPaths.Dtos
{
    public class IntegrationStatusDto
    {
        public bool IsLinked { get; set; }
        public bool HasLearningPath { get; set; }
        public Guid? LearningPathId { get; set; }
        public int TopicProgress { get; set; }
        public int SkillProficiency { get; set; }
        public string TopicStatus { get; set; }
        public string SkillStatus { get; set; }
        public int OverallProgress { get; set; }
    }
}
