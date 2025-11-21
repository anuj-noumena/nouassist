using System;
using NouAssist.BusinessOperations.FreelanceOperations.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.FreelanceOperations.Events
{
    /// <summary>
    /// Event raised when a proposal is sent to a client
    /// </summary>
    [EventName("NouAssist.BusinessOperations.FreelanceOperations.ProposalSent")]
    public class ProposalSentEto : ProposalEtoBase
    {
        /// <summary>
        /// Time when the proposal was sent
        /// </summary>
        public DateTime SentAt { get; set; }
        
        /// <summary>
        /// Email address the proposal was sent to
        /// </summary>
        public string SentToEmail { get; set; }
    }
}
