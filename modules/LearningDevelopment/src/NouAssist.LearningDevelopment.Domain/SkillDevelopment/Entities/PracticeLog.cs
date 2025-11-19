using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Entities
{
    public class PracticeLog : Entity
    {
        public Guid MilestoneId { get; private set; }
        public Guid PracticeLogId { get; private set; }
        public DateTime Date { get; private set; }
        public int DurationMinutes { get; private set; }
        public string FocusArea { get; private set; }
        public int? SelfRating { get; private set; } // 1-10
        public List<string> ChallengesFaced { get; private set; }
        public string Notes { get; private set; }

        protected PracticeLog() 
        { 
            ChallengesFaced = new List<string>();
        }

        public PracticeLog(
            Guid milestoneId, 
            DateTime date, 
            int durationMinutes, 
            string focusArea = null)
        {
            MilestoneId = milestoneId;
            PracticeLogId = Guid.NewGuid();
            Date = date;
            DurationMinutes = durationMinutes > 0 ? durationMinutes : 
                throw new ArgumentException("Duration must be positive", nameof(durationMinutes));
            FocusArea = focusArea;
            ChallengesFaced = new List<string>();
        }

        public void AddRating(int rating)
        {
            if (rating < 1 || rating > 10)
            {
                throw new BusinessException("SkillDevelopment:InvalidRating")
                    .WithData("Rating", rating);
            }

            SelfRating = rating;
        }

        public void AddChallenge(string challenge)
        {
            if (!string.IsNullOrWhiteSpace(challenge))
            {
                ChallengesFaced.Add(challenge);
            }
        }

        public void AddNotes(string notes)
        {
            Notes = notes;
        }

        public override object[] GetKeys() => new object[] { MilestoneId, PracticeLogId };
    }
}
