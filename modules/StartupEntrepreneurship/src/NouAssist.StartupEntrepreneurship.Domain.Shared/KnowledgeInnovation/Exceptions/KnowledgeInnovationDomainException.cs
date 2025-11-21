using Volo.Abp;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Exceptions
{
    /// <summary>
    /// Base exception for Knowledge & Innovation domain
    /// </summary>
    public class KnowledgeInnovationDomainException : BusinessException
    {
        public KnowledgeInnovationDomainException(string code)
            : base(code)
        {
        }

        public KnowledgeInnovationDomainException(string code, string message)
            : base(code, message)
        {
        }

        public KnowledgeInnovationDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
