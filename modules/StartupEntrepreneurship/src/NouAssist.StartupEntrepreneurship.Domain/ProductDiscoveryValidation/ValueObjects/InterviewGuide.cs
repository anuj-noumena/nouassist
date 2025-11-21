using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects
{
    /// <summary>
    /// Value object representing a structured interview guide
    /// </summary>
    public class InterviewGuide : ValueObject
    {
        public string Objective { get; private set; }
        public List<string> Questions { get; private set; }
        public int EstimatedDuration { get; private set; } // in minutes

        private InterviewGuide() 
        { 
            Questions = new List<string>();
        }

        public InterviewGuide(string objective, List<string> questions, int estimatedDuration)
        {
            SetObjective(objective);
            SetQuestions(questions);
            SetEstimatedDuration(estimatedDuration);
        }

        private void SetObjective(string objective)
        {
            Objective = Check.NotNullOrWhiteSpace(
                objective,
                nameof(objective),
                maxLength: ProductDiscoveryConsts.MaxInterviewDescriptionLength
            );
        }

        private void SetQuestions(List<string> questions)
        {
            Questions = questions ?? throw new ArgumentNullException(nameof(questions));
            
            if (Questions.Count == 0)
            {
                throw new BusinessException("ProductDiscovery:Validation:Required", "At least one question is required");
            }

            foreach (var question in Questions)
            {
                if (string.IsNullOrWhiteSpace(question))
                {
                    throw new BusinessException("ProductDiscovery:Validation:Required", "Question cannot be empty");
                }
            }
        }

        private void SetEstimatedDuration(int estimatedDuration)
        {
            if (estimatedDuration <= 0 || estimatedDuration > 480) // Max 8 hours
            {
                throw new BusinessException("ProductDiscovery:Validation:InvalidRange", 
                    "Duration must be between 1 and 480 minutes");
            }

            EstimatedDuration = estimatedDuration;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Objective;
            yield return EstimatedDuration;
            foreach (var question in Questions)
            {
                yield return question;
            }
        }
    }
}
