using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.Elicitation.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.Elicitation.Events
{
    /// <summary>
    /// Event published when an elicitation session is completed
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.Elicitation.ElicitationSessionCompleted")]
    public class ElicitationSessionCompletedEto : ElicitationEtoBase
    {
        public string Title { get; set; }
        public SessionType Type { get; set; }
        public DateTime CompletedAt { get; set; }
        public List<Guid> ParticipantIds { get; set; } = new();
        public int ArtifactsGenerated { get; set; }
    }
}
