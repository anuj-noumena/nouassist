using System.Collections.Generic;
using NouAssist.RequirementsManagement.ChangeManagement.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.ChangeManagement.ValueObjects
{
    /// <summary>
    /// Represents the role of an approver in the change management process
    /// </summary>
    public class ApproverRole : ValueObject
    {
        public string Name { get; private set; }
        public int Priority { get; private set; }

        private ApproverRole() { }

        public ApproverRole(string name, int priority = 0)
        {
            SetName(name);
            Priority = priority;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ChangeManagementConsts.MaxApproverRoleLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Priority;
        }

        public override string ToString()
        {
            return Name;
        }

        // Common approver roles
        public static ApproverRole ProductManager => new ApproverRole("Product Manager", 1);
        public static ApproverRole TechnicalLead => new ApproverRole("Technical Lead", 2);
        public static ApproverRole BusinessAnalyst => new ApproverRole("Business Analyst", 3);
        public static ApproverRole QualityAssurance => new ApproverRole("Quality Assurance", 4);
    }
}
