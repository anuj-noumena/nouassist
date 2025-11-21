using Volo.Abp;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Exceptions
{
    /// <summary>
    /// Base exception class for GrowthExperimentation domain exceptions
    /// </summary>
    public class GrowthExperimentationDomainException : BusinessException
    {
        public GrowthExperimentationDomainException(string code)
            : base(code)
        {
        }

        public GrowthExperimentationDomainException(string code, string message)
            : base(code, message)
        {
        }

        public GrowthExperimentationDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
