using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Learning.Aggregates
{
    /// <summary>
    /// Aggregate root for visual knowledge graph (mindmap)
    /// </summary>
    public class Mindmap : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid TopicId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Url { get; private set; }

        private readonly Dictionary<string, object> _data = new();
        public IReadOnlyDictionary<string, object> Data => _data;

        protected Mindmap() { /* ORM */ }

        public Mindmap(Guid id, Guid userId, Guid topicId, Guid? tenantId = null)
            : base(id)
        {
            UserId = Check.NotNull(userId, nameof(userId));
            TopicId = Check.NotNull(topicId, nameof(topicId));
            TenantId = tenantId;
        }

        public void UpdateData(Dictionary<string, object> data)
        {
            if (data == null || data.Count == 0)
            {
                throw new BusinessException("Learning:InvalidMindmapData");
            }

            _data.Clear();
            foreach (var kvp in data)
            {
                _data[kvp.Key] = kvp.Value;
            }
        }

        public void UpdateUrl(string url)
        {
            Url = url;
        }

        public void AddNode(string nodeId, object nodeData)
        {
            if (!_data.ContainsKey("nodes"))
            {
                _data["nodes"] = new Dictionary<string, object>();
            }

            var nodes = _data["nodes"] as Dictionary<string, object>;
            nodes[nodeId] = nodeData;
        }

        public void RemoveNode(string nodeId)
        {
            if (_data.ContainsKey("nodes"))
            {
                var nodes = _data["nodes"] as Dictionary<string, object>;
                nodes?.Remove(nodeId);
            }
        }

        public void AddEdge(string edgeId, object edgeData)
        {
            if (!_data.ContainsKey("edges"))
            {
                _data["edges"] = new Dictionary<string, object>();
            }

            var edges = _data["edges"] as Dictionary<string, object>;
            edges[edgeId] = edgeData;
        }

        public void RemoveEdge(string edgeId)
        {
            if (_data.ContainsKey("edges"))
            {
                var edges = _data["edges"] as Dictionary<string, object>;
                edges?.Remove(edgeId);
            }
        }
    }
}
