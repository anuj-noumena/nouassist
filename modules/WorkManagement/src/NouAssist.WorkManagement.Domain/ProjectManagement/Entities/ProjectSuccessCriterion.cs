using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.ProjectManagement.Entities
{
    public class ProjectSuccessCriterion : FullAuditedEntity<Guid>
    {
        public Guid ProjectId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; set; }
        public SuccessCriterionType CriterionType { get; private set; }
        
        // Measurement
        public string MeasurementMethod { get; set; }
        public string TargetValue { get; set; }
        public string ActualValue { get; set; }
        public string Unit { get; set; }
        public double? TargetNumeric { get; set; }
        public double? ActualNumeric { get; set; }
        
        // Status
        public double Weight { get; private set; }
        public int OrderIndex { get; set; }
        public bool IsMet { get; private set; }
        public DateTime? TargetDate { get; set; }
        public DateTime? MetAt { get; private set; }
        public string MetBy { get; private set; }
        
        // Documentation
        public string Notes { get; set; }
        public string EvidenceUrl { get; set; }
        
        private readonly List<Guid> _attachmentIds = new();
        public IReadOnlyCollection<Guid> AttachmentIds => _attachmentIds.AsReadOnly();
        
        private Dictionary<string, object> _metadata;
        public IReadOnlyDictionary<string, object> Metadata => 
            _metadata?.AsReadOnly();

        protected ProjectSuccessCriterion() { }

        public ProjectSuccessCriterion(
            Guid id,
            Guid projectId,
            string title,
            SuccessCriterionType criterionType,
            string description = null,
            string measurementMethod = null,
            string targetValue = null,
            double weight = 1.0) : base(id)
        {
            ProjectId = projectId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            CriterionType = criterionType;
            Description = description;
            MeasurementMethod = measurementMethod;
            TargetValue = targetValue;
            SetWeight(weight);
            IsMet = false;
        }

        public void SetWeight(double weight)
        {
            if (weight < 0 || weight > 10)
            {
                throw new BusinessException("Projects:InvalidWeight",
                    "Weight must be between 0 and 10.");
            }
            Weight = weight;
        }

        public void MarkAsMet(string metBy, string notes = null, string evidenceUrl = null)
        {
            if (IsMet)
            {
                return;
            }

            IsMet = true;
            MetAt = DateTime.UtcNow;
            MetBy = Check.NotNullOrWhiteSpace(metBy, nameof(metBy));
            
            if (!string.IsNullOrWhiteSpace(notes))
            {
                Notes = notes;
            }

            if (!string.IsNullOrWhiteSpace(evidenceUrl))
            {
                EvidenceUrl = evidenceUrl;
            }
        }

        public void UnmarkAsMet()
        {
            IsMet = false;
            MetAt = null;
            MetBy = null;
        }

        public void UpdateActualValue(string actualValue = null, double? actualNumeric = null)
        {
            ActualValue = actualValue;
            ActualNumeric = actualNumeric;
        }

        public void AddAttachment(Guid attachmentId)
        {
            if (!_attachmentIds.Contains(attachmentId))
            {
                _attachmentIds.Add(attachmentId);
            }
        }

        public void RemoveAttachment(Guid attachmentId)
        {
            _attachmentIds.Remove(attachmentId);
        }
        
        public void SetMetadata(string key, object value)
        {
            if (_metadata == null)
            {
                _metadata = new Dictionary<string, object>();
            }

            _metadata[key] = value;
        }

        // Computed Properties
        public bool HasTarget => !string.IsNullOrWhiteSpace(TargetValue) || TargetNumeric.HasValue;
        public bool HasActual => !string.IsNullOrWhiteSpace(ActualValue) || ActualNumeric.HasValue;
        public bool HasTargetDate => TargetDate.HasValue;
        public bool HasEvidence => !string.IsNullOrWhiteSpace(EvidenceUrl) || _attachmentIds.Any();
        
        public bool IsOverdue => TargetDate.HasValue && 
                                 DateTime.UtcNow > TargetDate.Value && 
                                 !IsMet;

        public bool IsQuantitative => CriterionType == SuccessCriterionType.Quantitative;
        public bool IsQualitative => CriterionType == SuccessCriterionType.Qualitative;
        public bool IsBinary => CriterionType == SuccessCriterionType.Binary;

        public double? AchievementPercentage
        {
            get
            {
                if (!TargetNumeric.HasValue || !ActualNumeric.HasValue)
                {
                    return null;
                }

                if (TargetNumeric.Value == 0)
                {
                    return null;
                }

                return Math.Clamp(ActualNumeric.Value / TargetNumeric.Value * 100, 0, 100);
            }
        }
    }
}
