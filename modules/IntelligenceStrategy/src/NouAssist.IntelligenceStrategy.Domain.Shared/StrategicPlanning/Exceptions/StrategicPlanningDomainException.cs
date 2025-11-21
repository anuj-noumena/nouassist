using Volo.Abp;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Exceptions
{
    /// <summary>
    /// Base exception for Strategic Planning domain
    /// </summary>
    public class StrategicPlanningDomainException : BusinessException
    {
        public StrategicPlanningDomainException(string code)
            : base(code)
        {
        }

        public StrategicPlanningDomainException(string code, string message)
            : base(code, message)
        {
        }

        public StrategicPlanningDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
