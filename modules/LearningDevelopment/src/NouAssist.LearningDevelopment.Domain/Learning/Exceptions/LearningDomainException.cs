using System;
using Volo.Abp;

namespace NouAssist.LearningDevelopment.Learning.Exceptions
{
    public class LearningDomainException : BusinessException
    {
        public LearningDomainException(string code, string? message = null, string? details = null, Exception? innerException = null)
            : base(code, message, details, innerException)
        {
        }

        public static LearningDomainException SessionAlreadyEnded()
        {
            return new LearningDomainException(
                "Learning:SessionAlreadyEnded",
                "Learning session has already ended.");
        }

        public static LearningDomainException InvalidBloomLevel()
        {
            return new LearningDomainException(
                "Learning:InvalidBloomLevel",
                "Invalid Bloom's taxonomy level specified.");
        }

        public static LearningDomainException TopicAlreadyMastered()
        {
            return new LearningDomainException(
                "Learning:TopicAlreadyMastered",
                "Topic has already been marked as mastered.");
        }

        public static LearningDomainException InvalidCyclePhase()
        {
            return new LearningDomainException(
                "Learning:InvalidCyclePhase",
                "Cannot perform this operation in the current learning cycle phase.");
        }

        public static LearningDomainException MindmapNodeNotFound(Guid nodeId)
        {
            return new LearningDomainException(
                "Learning:MindmapNodeNotFound",
                $"Mindmap node with ID {nodeId} not found.");
        }
    }
}
