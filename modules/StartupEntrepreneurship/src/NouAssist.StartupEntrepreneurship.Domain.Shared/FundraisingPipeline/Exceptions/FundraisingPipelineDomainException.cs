namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Exceptions
{
    /// <summary>
    /// Base exception for FundraisingPipeline domain
    /// </summary>
    public class FundraisingPipelineDomainException : BusinessException
    {
        public FundraisingPipelineDomainException(string code)
            : base(code)
        {
        }

        public FundraisingPipelineDomainException(string code, string message)
            : base(code, message)
        {
        }

        public FundraisingPipelineDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
