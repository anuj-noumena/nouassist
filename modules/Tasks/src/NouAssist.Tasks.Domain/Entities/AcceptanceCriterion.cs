using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Timing;

namespace NouAssist.Tasks.Entities
{
    public class AcceptanceCriterion : FullAuditedEntity<Guid>
    {
        public Guid TaskId { get; private set; }
        public int? Priority { get; private set; }
        public string Description { get; private set; }
        public string GivenContext { get; private set; }
        public string WhenAction { get; private set; }
        public string ThenOutcome { get; private set; }
        public string TestScenario { get; set; }
        public string Notes { get; set; }

        protected AcceptanceCriterion()
        {
        }

        public AcceptanceCriterion(
            Guid taskId,
            string description,
            int? priority = null,
            string givenContext = null,
            string whenAction = null,
            string thenOutcome = null,
            string testScenario = null)
        {
            Id = Guid.NewGuid();
            TaskId = taskId;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 1000);
            Priority = priority;
            GivenContext = givenContext;
            WhenAction = whenAction;
            ThenOutcome = thenOutcome;
            TestScenario = testScenario;
        }

        public bool HasGivenWhenThen =>
            !string.IsNullOrWhiteSpace(GivenContext) &&
            !string.IsNullOrWhiteSpace(WhenAction) &&
            !string.IsNullOrWhiteSpace(ThenOutcome);

        public string GetFormattedGWT()
        {
            if (!HasGivenWhenThen)
            {
                return Description;
            }

            return $"GIVEN {GivenContext}\nWHEN {WhenAction}\nTHEN {ThenOutcome}";
        }

        public void Update(
            string description,
            int? priority = null,
            string givenContext = null,
            string whenAction = null,
            string thenOutcome = null,
            string testScenario = null)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 1000);
            Priority = priority;
            GivenContext = givenContext;
            WhenAction = whenAction;
            ThenOutcome = thenOutcome;
            TestScenario = testScenario;
        }
    }
}