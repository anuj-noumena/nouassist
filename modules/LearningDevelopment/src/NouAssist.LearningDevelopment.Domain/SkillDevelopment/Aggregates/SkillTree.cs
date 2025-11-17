using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Aggregates
{
    public class SkillTree : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid SkillId { get; private set; }
        public Guid? TenantId { get; private set; }
        
        // Store as JSON structure: {nodes: [...], edges: [...]}
        private Dictionary<string, object> _structure = new();
        public IReadOnlyDictionary<string, object> Structure => _structure;
        
        public string VisualizationUrl { get; private set; }

        protected SkillTree() { /* ORM */ }

        public SkillTree(
            Guid id,
            Guid userId,
            Guid skillId,
            Guid? tenantId = null)
            : base(id)
        {
            UserId = userId;
            SkillId = skillId;
            TenantId = tenantId;
            _structure = new Dictionary<string, object>
            {
                { "nodes", new List<object>() },
                { "edges", new List<object>() }
            };
        }

        public void UpdateStructure(Dictionary<string, object> structure)
        {
            _structure = structure ?? throw new ArgumentNullException(nameof(structure));
        }

        public void SetVisualizationUrl(string url)
        {
            VisualizationUrl = url;
        }

        public void AddNode(Guid nodeId, string label, Dictionary<string, object> properties = null)
        {
            var nodes = _structure["nodes"] as List<object> ?? new List<object>();
            nodes.Add(new Dictionary<string, object>
            {
                { "id", nodeId.ToString() },
                { "label", label },
                { "properties", properties ?? new Dictionary<string, object>() }
            });
            _structure["nodes"] = nodes;
        }

        public void AddEdge(Guid fromNodeId, Guid toNodeId, string relationship)
        {
            var edges = _structure["edges"] as List<object> ?? new List<object>();
            edges.Add(new Dictionary<string, object>
            {
                { "from", fromNodeId.ToString() },
                { "to", toNodeId.ToString() },
                { "relationship", relationship }
            });
            _structure["edges"] = edges;
        }
    }
}
