using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.ValueObjects
{
    /// <summary>
    /// Value object representing a BPMN diagram
    /// </summary>
    public class BpmnDiagram : ValueObject
    {
        /// <summary>
        /// The XML content of the BPMN diagram
        /// </summary>
        public string XmlContent { get; private set; }
        
        /// <summary>
        /// The BPMN version used
        /// </summary>
        public string BpmnVersion { get; private set; }
        
        /// <summary>
        /// The timestamp when the diagram was last updated
        /// </summary>
        public DateTime LastUpdated { get; private set; }

        protected BpmnDiagram() { }

        public BpmnDiagram(string xmlContent, string bpmnVersion)
        {
            SetXmlContent(xmlContent);
            BpmnVersion = bpmnVersion;
            LastUpdated = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Updates the BPMN diagram content
        /// </summary>
        /// <param name="xmlContent">The new XML content</param>
        public void UpdateContent(string xmlContent)
        {
            SetXmlContent(xmlContent);
            LastUpdated = DateTime.UtcNow;
        }
        
        private void SetXmlContent(string xmlContent)
        {
            XmlContent = Check.NotNullOrWhiteSpace(
                xmlContent,
                nameof(xmlContent),
                maxLength: WorkflowEngineConsts.MaxBpmnDiagramLength
            );
            
            // Additional validation could be performed here to ensure 
            // the XML is valid BPMN format
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return XmlContent;
            yield return BpmnVersion;
            yield return LastUpdated;
        }
    }
}
