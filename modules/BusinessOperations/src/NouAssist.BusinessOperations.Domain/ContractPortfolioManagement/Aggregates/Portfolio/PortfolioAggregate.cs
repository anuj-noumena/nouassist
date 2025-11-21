using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Constants;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Portfolio;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Events;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Exceptions;
using NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Portfolio
{
    /// <summary>
    /// Represents a portfolio containing multiple items (contracts, projects, etc.)
    /// </summary>
    public class PortfolioAggregate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        #region Properties
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<PortfolioGoal> _goals;
        public IReadOnlyCollection<PortfolioGoal> Goals => _goals.AsReadOnly();

        private readonly List<PerformanceMetric> _metrics;
        public IReadOnlyCollection<PerformanceMetric> Metrics => _metrics.AsReadOnly();

        private readonly List<PortfolioItem> _items;
        public IReadOnlyCollection<PortfolioItem> Items => _items.AsReadOnly();

        private readonly List<PortfolioCategory> _categories;
        public IReadOnlyCollection<PortfolioCategory> Categories => _categories.AsReadOnly();

        private readonly List<PortfolioRisk> _risks;
        public IReadOnlyCollection<PortfolioRisk> Risks => _risks.AsReadOnly();

        private readonly List<PortfolioComplianceRecord> _complianceRecords;
        public IReadOnlyCollection<PortfolioComplianceRecord> ComplianceRecords => _complianceRecords.AsReadOnly();

        private readonly List<SharingLink> _sharingLinks;
        public IReadOnlyCollection<SharingLink> SharingLinks => _sharingLinks.AsReadOnly();
        #endregion

        #region Constructors
        protected PortfolioAggregate()
        {
            _goals = new List<PortfolioGoal>();
            _metrics = new List<PerformanceMetric>();
            _items = new List<PortfolioItem>();
            _categories = new List<PortfolioCategory>();
            _risks = new List<PortfolioRisk>();
            _complianceRecords = new List<PortfolioComplianceRecord>();
            _sharingLinks = new List<SharingLink>();
        }

        public static PortfolioAggregate Create(
            Guid id,
            Guid? tenantId,
            string name,
            string description)
        {
            return new PortfolioAggregate(id, tenantId, name, description);
        }

        private PortfolioAggregate(
            Guid id,
            Guid? tenantId,
            string name,
            string description)
            : base(id)
        {
            TenantId = tenantId;
            SetName(name);
            SetDescription(description);
            IsActive = true;

            _goals = new List<PortfolioGoal>();
            _metrics = new List<PerformanceMetric>();
            _items = new List<PortfolioItem>();
            _categories = new List<PortfolioCategory>();
            _risks = new List<PortfolioRisk>();
            _complianceRecords = new List<PortfolioComplianceRecord>();
            _sharingLinks = new List<SharingLink>();

            // Add domain event
            AddDistributedEvent(new PortfolioCreatedEto
            {
                Id = Id,
                Name = Name,
                Description = Description,
                TenantId = TenantId
            });
        }
        #endregion

        #region Public Methods
        public void UpdateBasicDetails(string name, string description)
        {
            SetName(name);
            SetDescription(description);
        }

        public void SetActiveStatus(bool isActive)
        {
            IsActive = isActive;
        }

        public void AddGoal(string title, string description, DateTime targetDate)
        {
            var goal = new PortfolioGoal(title, description, targetDate);
            _goals.Add(goal);
        }

        public void RemoveGoal(PortfolioGoal goal)
        {
            _goals.Remove(goal);
        }

        public void AddMetric(string name, string description, decimal target, decimal actual, string unit)
        {
            var metric = new PerformanceMetric(name, description, target, actual, unit, DateTime.Now);
            _metrics.Add(metric);
        }

        public void UpdateMetric(string name, decimal actual, DateTime updateTime)
        {
            var metric = _metrics.FirstOrDefault(m => m.Name == name);
            if (metric != null)
            {
                metric.UpdateActualValue(actual, updateTime);
            }
        }

        public void RemoveMetric(string name)
        {
            var metric = _metrics.FirstOrDefault(m => m.Name == name);
            if (metric != null)
            {
                _metrics.Remove(metric);
            }
        }

        public PortfolioItem AddItem(ItemReference itemReference, Guid? categoryId = null)
        {
            // Check if item already exists
            if (_items.Any(i => i.Reference.ItemId == itemReference.ItemId && 
                               i.Reference.ItemType == itemReference.ItemType))
            {
                throw new DuplicatePortfolioItemException(Id, itemReference.ItemId, itemReference.ItemType);
            }

            var item = new PortfolioItem(
                GuidGenerator.Create(),
                Id,
                itemReference,
                categoryId);

            _items.Add(item);

            // Add domain event
            AddDistributedEvent(new PortfolioItemAddedEto
            {
                PortfolioId = Id,
                ItemId = itemReference.ItemId,
                ItemType = itemReference.ItemType,
                ItemName = itemReference.ItemName,
                TenantId = TenantId
            });

            return item;
        }

        public void RemoveItem(Guid itemId)
        {
            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                _items.Remove(item);
            }
        }

        public PortfolioCategory AddCategory(string name, string description, string colorCode = null)
        {
            var category = new PortfolioCategory(
                GuidGenerator.Create(),
                Id,
                name,
                description,
                colorCode);

            _categories.Add(category);
            return category;
        }

        public void RemoveCategory(Guid categoryId)
        {
            var category = _categories.FirstOrDefault(c => c.Id == categoryId);
            if (category != null)
            {
                // Update any items that use this category
                foreach (var item in _items.Where(i => i.CategoryId == categoryId))
                {
                    item.UpdateCategory(null);
                }

                _categories.Remove(category);
            }
        }

        public PortfolioRisk AddRisk(
            string title,
            RiskAssessmentData assessmentData)
        {
            var risk = new PortfolioRisk(
                GuidGenerator.Create(),
                Id,
                title,
                assessmentData);

            _risks.Add(risk);
            return risk;
        }

        public void UpdateRiskAssessment(
            Guid riskId,
            RiskAssessmentData assessmentData)
        {
            var risk = _risks.FirstOrDefault(r => r.Id == riskId);
            if (risk != null)
            {
                risk.UpdateAssessment(assessmentData);
            }
        }

        public void RemoveRisk(Guid riskId)
        {
            var risk = _risks.FirstOrDefault(r => r.Id == riskId);
            if (risk != null)
            {
                _risks.Remove(risk);
            }
        }

        public PortfolioComplianceRecord AddComplianceRecord(
            ComplianceRule rule,
            bool isCompliant,
            string evidenceDescription,
            DateTime assessmentDate)
        {
            var record = new PortfolioComplianceRecord(
                GuidGenerator.Create(),
                Id,
                rule,
                isCompliant,
                evidenceDescription,
                assessmentDate);

            _complianceRecords.Add(record);
            return record;
        }

        public void UpdateComplianceStatus(
            Guid recordId,
            bool isCompliant,
            string evidenceDescription,
            DateTime assessmentDate)
        {
            var record = _complianceRecords.FirstOrDefault(r => r.Id == recordId);
            if (record != null)
            {
                record.UpdateStatus(isCompliant, evidenceDescription, assessmentDate);
            }
        }

        public void RemoveComplianceRecord(Guid recordId)
        {
            var record = _complianceRecords.FirstOrDefault(r => r.Id == recordId);
            if (record != null)
            {
                _complianceRecords.Remove(record);
            }
        }

        public SharingLink CreateSharingLink(
            DateTime expirationDate,
            AccessPermissions permissions)
        {
            var linkId = GuidGenerator.Create();
            var token = GenerateSharingToken();

            var sharingLink = new SharingLink(
                linkId,
                token,
                expirationDate,
                permissions);

            _sharingLinks.Add(sharingLink);
            return sharingLink;
        }

        public void RemoveSharingLink(Guid linkId)
        {
            var link = _sharingLinks.FirstOrDefault(l => l.LinkId == linkId);
            if (link != null)
            {
                _sharingLinks.Remove(link);
            }
        }

        public void PurgeExpiredSharingLinks(DateTime currentTime)
        {
            var expiredLinks = _sharingLinks.Where(l => l.IsExpired(currentTime)).ToList();
            foreach (var link in expiredLinks)
            {
                _sharingLinks.Remove(link);
            }
        }

        public decimal CalculateOverallCompliancePercentage()
        {
            if (!_complianceRecords.Any())
            {
                return 100; // No compliance requirements means 100% compliant
            }

            var totalRecords = _complianceRecords.Count;
            var compliantRecords = _complianceRecords.Count(r => r.IsCompliant);

            return ((decimal)compliantRecords / totalRecords) * 100;
        }

        public decimal CalculateOverallRiskScore()
        {
            if (!_risks.Any())
            {
                return 0; // No risks means risk score is 0
            }

            return _risks.Average(r => r.Assessment.CalculateRiskScore());
        }

        public PortfolioItem GetItem(Guid itemId)
        {
            return _items.FirstOrDefault(i => i.Id == itemId);
        }

        public PortfolioCategory GetCategory(Guid categoryId)
        {
            return _categories.FirstOrDefault(c => c.Id == categoryId);
        }
        #endregion

        #region Private Methods
        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ContractPortfolioManagementConsts.Portfolio.MaxNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: ContractPortfolioManagementConsts.Portfolio.MaxDescriptionLength
            );
        }

        private string GenerateSharingToken()
        {
            // In real implementation, this would be a secure token generator
            return Guid.NewGuid().ToString("N");
        }
        #endregion
    }
}
