using NouAssist.BusinessOperations.FreelanceOperations.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.FreelanceOperations.Events
{
    /// <summary>
    /// Event raised when a new freelance client is created
    /// </summary>
    [EventName("NouAssist.BusinessOperations.FreelanceOperations.ClientCreated")]
    public class FreelanceClientCreatedEto : ClientEtoBase
    {
        /// <summary>
        /// Company name of the client
        /// </summary>
        public string CompanyName { get; set; }
    }
}
