using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Events;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Exceptions;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates
{
    /// <summary>
    /// Revenue Model aggregate root
    /// Manages revenue models, pricing strategies, and financial projections
    /// </summary>
    public class RevenueModel : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public RevenueModelType Type { get; private set; }
        public Guid OwnerId { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<PricingModel> _pricingModels;
        public IReadOnlyCollection<PricingModel> PricingModels => _pricingModels.AsReadOnly();

        private readonly List<RevenueScenario> _scenarios;
        public IReadOnlyCollection<RevenueScenario> Scenarios => _scenarios.AsReadOnly();

        private readonly List<GoToMarketStrategy> _gtmStrategies;
        public IReadOnlyCollection<GoToMarketStrategy> GtmStrategies => _gtmStrategies.AsReadOnly();

        protected RevenueModel()
        {
            _pricingModels = new List<PricingModel>();
            _scenarios = new List<RevenueScenario>();
            _gtmStrategies = new List<GoToMarketStrategy>();
        }

        public static RevenueModel Create(
            Guid id,
            string name,
            string description,
            RevenueModelType type,
            Guid ownerId,
            Guid? tenantId)
        {
            return new RevenueModel(id, name, description, type, ownerId, tenantId);
        }

        private RevenueModel(
            Guid id,
            string name,
            string description,
            RevenueModelType type,
            Guid ownerId,
            Guid? tenantId) : base(id)
        {
            SetName(name);
            SetDescription(description);
            Type = type;
            OwnerId = ownerId;
            TenantId = tenantId;
            IsActive = true;
            _pricingModels = new List<PricingModel>();
            _scenarios = new List<RevenueScenario>();
            _gtmStrategies = new List<GoToMarketStrategy>();

            AddDistributedEvent(new RevenueModelCreatedEto
            {
                Id = Id,
                Name = Name,
                Type = Type,
                CreatedBy = OwnerId,
                TenantId = TenantId
            });
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: BusinessModelManagementConsts.RevenueModel.MaxNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNull(
                description,
                nameof(description),
                maxLength: BusinessModelManagementConsts.RevenueModel.MaxDescriptionLength
            );
        }

        public void UpdateName(string name)
        {
            SetName(name);
        }

        public void UpdateDescription(string description)
        {
            SetDescription(description);
        }

        public void UpdateType(RevenueModelType type)
        {
            Type = type;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public PricingModel AddPricingModel(string name, PricingStrategy strategy, string description = null)
        {
            if (_pricingModels.Any(pm => pm.Name == name))
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:DuplicateName")
                    .WithData("entityType", "PricingModel")
                    .WithData("name", name);
            }

            var pricingModel = new PricingModel(
                Guid.NewGuid(),
                Id,
                name,
                strategy,
                description
            );

            _pricingModels.Add(pricingModel);
            return pricingModel;
        }

        public void RemovePricingModel(Guid pricingModelId)
        {
            var pricingModel = _pricingModels.FirstOrDefault(pm => pm.Id == pricingModelId);
            if (pricingModel != null)
            {
                _pricingModels.Remove(pricingModel);
            }
        }

        public RevenueScenario CreateScenario(
            string name,
            string description,
            ScenarioParameters parameters,
            Guid createdBy,
            DateTime creationTime)
        {
            if (_scenarios.Any(s => s.Name == name))
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:DuplicateName")
                    .WithData("entityType", "RevenueScenario")
                    .WithData("name", name);
            }

            var scenario = new RevenueScenario(
                Guid.NewGuid(),
                Id,
                name,
                description,
                parameters,
                createdBy,
                creationTime
            );

            _scenarios.Add(scenario);
            return scenario;
        }

        public void RemoveScenario(Guid scenarioId)
        {
            var scenario = _scenarios.FirstOrDefault(s => s.Id == scenarioId);
            if (scenario != null)
            {
                _scenarios.Remove(scenario);
            }
        }

        public GoToMarketStrategy AddGtmStrategy(
            string name,
            string description,
            string targetMarket,
            DateTime startDate,
            DateTime? endDate = null)
        {
            if (_gtmStrategies.Any(gtm => gtm.Name == name))
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:DuplicateName")
                    .WithData("entityType", "GoToMarketStrategy")
                    .WithData("name", name);
            }

            var gtmStrategy = new GoToMarketStrategy(
                Guid.NewGuid(),
                Id,
                name,
                description,
                targetMarket,
                startDate,
                endDate
            );

            _gtmStrategies.Add(gtmStrategy);
            return gtmStrategy;
        }

        public void RemoveGtmStrategy(Guid gtmStrategyId)
        {
            var gtmStrategy = _gtmStrategies.FirstOrDefault(gtm => gtm.Id == gtmStrategyId);
            if (gtmStrategy != null)
            {
                _gtmStrategies.Remove(gtmStrategy);
            }
        }

        public PricingModel GetPricingModel(Guid pricingModelId)
        {
            return _pricingModels.FirstOrDefault(pm => pm.Id == pricingModelId);
        }

        public RevenueScenario GetScenario(Guid scenarioId)
        {
            return _scenarios.FirstOrDefault(s => s.Id == scenarioId);
        }

        public GoToMarketStrategy GetGtmStrategy(Guid gtmStrategyId)
        {
            return _gtmStrategies.FirstOrDefault(gtm => gtm.Id == gtmStrategyId);
        }

        public IReadOnlyCollection<PricingModel> GetActivePricingModels()
        {
            return _pricingModels.Where(pm => pm.IsActive).ToList().AsReadOnly();
        }

        public IReadOnlyCollection<GoToMarketStrategy> GetActiveGtmStrategies()
        {
            return _gtmStrategies.Where(gtm => gtm.IsActive).ToList().AsReadOnly();
        }

        public decimal CalculateProjectedRevenue(int months)
        {
            // Simple calculation based on active scenarios
            var activeScenarios = _scenarios.Where(s => s.Projections.Any()).ToList();
            if (!activeScenarios.Any())
                return 0m;

            var totalProjectedRevenue = 0m;
            foreach (var scenario in activeScenarios)
            {
                var latestProjection = scenario.Projections
                    .OrderByDescending(p => p.ProjectionDate)
                    .FirstOrDefault();

                if (latestProjection != null)
                {
                    totalProjectedRevenue += latestProjection.TotalRevenue;
                }
            }

            return totalProjectedRevenue / activeScenarios.Count; // Average
        }
    }
}
