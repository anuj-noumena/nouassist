using System;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Events
{
    [EventName("WorkflowEngine.Template.Created")]
    public class WorkflowTemplateCreatedEto : WorkflowEventBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
