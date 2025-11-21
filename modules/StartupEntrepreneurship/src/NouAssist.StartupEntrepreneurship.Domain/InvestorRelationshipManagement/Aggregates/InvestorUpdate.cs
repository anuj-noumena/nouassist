using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Events;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Exceptions;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Aggregates
{
    /// <summary>
    /// Aggregate root representing an investor update with sections, metrics, and distribution tracking
    /// </summary>
    public class InvestorUpdate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Title { get; private set; }
        public string Subject { get; private set; }
        public UpdateStatus Status { get; private set; }
        public DateTime? ScheduledDate { get; private set; }
        public DateTime? SentDate { get; private set; }
        public UpdateFrequency Frequency { get; private set; }
        public string TemplateId { get; private set; }

        // Collections within aggregate boundary
        private readonly List<UpdateSection> _sections;
        private readonly List<MetricSnapshot> _metrics;
        private readonly List<DistributionLog> _distributionLogs;
        private readonly List<EngagementRecord> _engagementRecords;

        public IReadOnlyCollection<UpdateSection> Sections => _sections.AsReadOnly();
        public IReadOnlyCollection<MetricSnapshot> Metrics => _metrics.AsReadOnly();
        public IReadOnlyCollection<DistributionLog> DistributionLogs => _distributionLogs.AsReadOnly();
        public IReadOnlyCollection<EngagementRecord> EngagementRecords => _engagementRecords.AsReadOnly();

        protected InvestorUpdate()
        {
            _sections = new List<UpdateSection>();
            _metrics = new List<MetricSnapshot>();
            _distributionLogs = new List<DistributionLog>();
            _engagementRecords = new List<EngagementRecord>();
        }

        public static InvestorUpdate Create(
            Guid id,
            string title,
            string subject,
            UpdateFrequency frequency,
            string templateId = null,
            Guid? tenantId = null)
        {
            return new InvestorUpdate(id, title, subject, frequency, templateId, tenantId);
        }

        private InvestorUpdate(
            Guid id,
            string title,
            string subject,
            UpdateFrequency frequency,
            string templateId = null,
            Guid? tenantId = null) : base(id)
        {
            SetTitle(title);
            SetSubject(subject);
            Status = UpdateStatus.Draft;
            Frequency = frequency;
            TemplateId = templateId;
            TenantId = tenantId;
            _sections = new List<UpdateSection>();
            _metrics = new List<MetricSnapshot>();
            _distributionLogs = new List<DistributionLog>();
            _engagementRecords = new List<EngagementRecord>();
        }

        public void UpdateTitle(string title)
        {
            EnsureCanBeModified();
            SetTitle(title);
        }

        public void UpdateSubject(string subject)
        {
            EnsureCanBeModified();
            SetSubject(subject);
        }

        public void AddSection(string title, UpdateContent content, int? order = null)
        {
            EnsureCanBeModified();
            
            var sectionOrder = order ?? _sections.Count + 1;
            var section = new UpdateSection(
                GuidGenerator.Create(),
                Id,
                title,
                content,
                sectionOrder
            );

            _sections.Add(section);
        }

        public void UpdateSection(Guid sectionId, string title = null, UpdateContent content = null, int? order = null)
        {
            EnsureCanBeModified();
            
            var section = _sections.FirstOrDefault(s => s.Id == sectionId);
            if (section == null)
                throw new BusinessException("IRM:ErrorMessage:EntityNotFound", "Section not found");

            if (title != null)
                section.UpdateTitle(title);
            
            if (content != null)
                section.UpdateContent(content);
            
            if (order.HasValue)
                section.UpdateOrder(order.Value);
        }

        public void RemoveSection(Guid sectionId)
        {
            EnsureCanBeModified();
            
            var section = _sections.FirstOrDefault(s => s.Id == sectionId);
            if (section != null)
            {
                _sections.Remove(section);
                ReorderSections();
            }
        }

        public void AddMetric(MetricValue metric, DateTime? snapshotDate = null)
        {
            EnsureCanBeModified();
            
            var snapshot = new MetricSnapshot(
                GuidGenerator.Create(),
                Id,
                metric,
                snapshotDate ?? DateTime.UtcNow
            );

            _metrics.Add(snapshot);
        }

        public void Schedule(DateTime scheduledDate)
        {
            if (Status != UpdateStatus.Draft)
                throw new InvalidUpdateStatusTransitionException(Status, UpdateStatus.Scheduled);

            if (scheduledDate <= DateTime.UtcNow)
                throw new BusinessException("IRM:ErrorMessage:InvalidOperation", "Scheduled date must be in the future");

            Status = UpdateStatus.Scheduled;
            ScheduledDate = scheduledDate;
        }

        public void Send(List<Guid> investorIds, DateTime sentDate)
        {
            if (Status != UpdateStatus.Scheduled && Status != UpdateStatus.Draft)
                throw new InvalidUpdateStatusTransitionException(Status, UpdateStatus.Sent);

            if (!investorIds.Any())
                throw new BusinessException("IRM:ErrorMessage:InvalidOperation", "Must specify at least one investor");

            // Create distribution logs for each investor
            foreach (var investorId in investorIds)
            {
                var distributionLog = new DistributionLog(
                    GuidGenerator.Create(),
                    Id,
                    investorId,
                    "email" // Default channel
                );
                _distributionLogs.Add(distributionLog);
            }

            Status = UpdateStatus.Sent;
            SentDate = sentDate;

            AddDistributedEvent(new InvestorUpdateSentEto
            {
                Id = Id,
                Title = Title,
                InvestorIds = investorIds,
                ScheduledDate = ScheduledDate ?? sentDate,
                Frequency = Frequency,
                TenantId = TenantId
            });
        }

        public void MarkAsDelivered(Guid investorId, DateTime deliveredAt)
        {
            var distributionLog = _distributionLogs.FirstOrDefault(d => d.InvestorId == investorId);
            distributionLog?.MarkAsDelivered(deliveredAt);
        }

        public void MarkAsRead(Guid investorId)
        {
            var distributionLog = _distributionLogs.FirstOrDefault(d => d.InvestorId == investorId);
            distributionLog?.MarkAsRead();
        }

        public void MarkAsFailed(Guid investorId, string errorMessage)
        {
            var distributionLog = _distributionLogs.FirstOrDefault(d => d.InvestorId == investorId);
            distributionLog?.MarkAsFailed(errorMessage);
        }

        public void TrackEngagement(
            Guid investorId,
            EngagementType engagementType,
            DateTime engagedAt,
            string details = null,
            string ipAddress = null,
            string userAgent = null)
        {
            var engagement = new EngagementRecord(
                GuidGenerator.Create(),
                Id,
                investorId,
                engagementType,
                engagedAt,
                details,
                ipAddress,
                userAgent
            );

            _engagementRecords.Add(engagement);

            AddDistributedEvent(new InvestorEngagementTrackedEto
            {
                Id = GuidGenerator.Create(),
                InvestorId = investorId,
                UpdateId = Id,
                EngagementType = engagementType,
                Details = details,
                TenantId = TenantId
            });
        }

        public int GetDeliveryCount()
        {
            return _distributionLogs.Count(d => d.Status == UpdateStatus.Delivered || d.Status == UpdateStatus.Read);
        }

        public int GetReadCount()
        {
            return _distributionLogs.Count(d => d.Status == UpdateStatus.Read);
        }

        public int GetEngagementCount(EngagementType? type = null)
        {
            return type.HasValue 
                ? _engagementRecords.Count(e => e.Type == type.Value)
                : _engagementRecords.Count;
        }

        public decimal GetEngagementRate()
        {
            var totalSent = _distributionLogs.Count;
            if (totalSent == 0) return 0;

            var uniqueEngagedInvestors = _engagementRecords.Select(e => e.InvestorId).Distinct().Count();
            return (decimal)uniqueEngagedInvestors / totalSent * 100;
        }

        private void EnsureCanBeModified()
        {
            if (Status != UpdateStatus.Draft)
                throw new BusinessException("IRM:ErrorMessage:InvalidOperation", "Cannot modify update that is not in draft status");
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: IRMConsts.InvestorUpdate.MaxTitleLength
            );
        }

        private void SetSubject(string subject)
        {
            Subject = Check.NotNullOrWhiteSpace(
                subject,
                nameof(subject),
                maxLength: IRMConsts.InvestorUpdate.MaxSubjectLength
            );
        }

        private void ReorderSections()
        {
            var orderedSections = _sections.OrderBy(s => s.Order).ToList();
            for (int i = 0; i < orderedSections.Count; i++)
            {
                orderedSections[i].UpdateOrder(i + 1);
            }
        }
    }
}
