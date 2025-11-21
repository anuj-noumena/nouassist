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
    /// Business Model Canvas aggregate root
    /// Manages canvas creation, versioning, and hypothesis tracking
    /// </summary>
    public class BusinessModelCanvas : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int CurrentVersionNumber { get; private set; }
        public Guid? CurrentVersionId { get; private set; }
        public Guid OwnerId { get; private set; }

        private readonly List<CanvasVersion> _versions;
        public IReadOnlyCollection<CanvasVersion> Versions => _versions.AsReadOnly();

        private readonly List<CanvasComponent> _components;
        public IReadOnlyCollection<CanvasComponent> Components => _components.AsReadOnly();

        protected BusinessModelCanvas()
        {
            _versions = new List<CanvasVersion>();
            _components = new List<CanvasComponent>();
        }

        public static BusinessModelCanvas Create(
            Guid id,
            string title,
            string description,
            Guid ownerId,
            Guid? tenantId,
            DateTime creationTime)
        {
            return new BusinessModelCanvas(id, title, description, ownerId, tenantId, creationTime);
        }

        private BusinessModelCanvas(
            Guid id,
            string title,
            string description,
            Guid ownerId,
            Guid? tenantId,
            DateTime creationTime) : base(id)
        {
            SetTitle(title);
            SetDescription(description);
            OwnerId = ownerId;
            TenantId = tenantId;
            CurrentVersionNumber = 0;
            _versions = new List<CanvasVersion>();
            _components = new List<CanvasComponent>();

            // Create initial version
            CreateInitialVersion(creationTime);

            AddDistributedEvent(new BusinessModelCanvasCreatedEto
            {
                Id = Id,
                Title = Title,
                Description = Description,
                CreatedBy = OwnerId,
                TenantId = TenantId
            });
        }

        private void CreateInitialVersion(DateTime creationTime)
        {
            var initialVersion = new CanvasVersion(
                Guid.NewGuid(),
                Id,
                "Initial Version",
                1,
                OwnerId,
                creationTime,
                "Initial canvas version",
                true
            );

            _versions.Add(initialVersion);
            CurrentVersionNumber = 1;
            CurrentVersionId = initialVersion.Id;

            // Initialize all canvas components
            InitializeComponents(initialVersion.Id, creationTime);
        }

        private void InitializeComponents(Guid versionId, DateTime creationTime)
        {
            foreach (CanvasComponentType componentType in Enum.GetValues<CanvasComponentType>())
            {
                var component = new CanvasComponent(
                    Guid.NewGuid(),
                    Id,
                    versionId,
                    componentType,
                    new ComponentContent("", new List<string>(), new Dictionary<string, string>()),
                    OwnerId,
                    creationTime
                );

                _components.Add(component);
            }
        }

        public void UpdateTitle(string title)
        {
            SetTitle(title);
        }

        public void UpdateDescription(string description)
        {
            SetDescription(description);
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: BusinessModelManagementConsts.BusinessModelCanvas.MaxTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNull(
                description,
                nameof(description),
                maxLength: BusinessModelManagementConsts.BusinessModelCanvas.MaxDescriptionLength
            );
        }

        public CanvasVersion CreateNewVersion(string versionName, string notes, Guid createdBy, DateTime creationTime)
        {
            if (_versions.Any(v => v.VersionName == versionName))
            {
                throw new DuplicateVersionException(versionName);
            }

            // Mark current version as not current
            var currentVersion = _versions.FirstOrDefault(v => v.IsCurrentVersion);
            currentVersion?.SetAsNotCurrent();

            // Create new version
            var newVersionNumber = CurrentVersionNumber + 1;
            var newVersion = new CanvasVersion(
                Guid.NewGuid(),
                Id,
                versionName,
                newVersionNumber,
                createdBy,
                creationTime,
                notes,
                true
            );

            _versions.Add(newVersion);
            CurrentVersionNumber = newVersionNumber;
            CurrentVersionId = newVersion.Id;

            // Copy components from current version to new version
            CopyComponentsToNewVersion(newVersion.Id, creationTime);

            AddDistributedEvent(new BusinessModelCanvasVersionCreatedEto
            {
                Id = newVersion.Id,
                CanvasId = Id,
                VersionName = versionName,
                VersionNumber = newVersionNumber,
                IsCurrentVersion = true,
                TenantId = TenantId
            });

            return newVersion;
        }

        private void CopyComponentsToNewVersion(Guid newVersionId, DateTime creationTime)
        {
            var currentComponents = _components.Where(c => c.VersionId == CurrentVersionId).ToList();

            foreach (var currentComponent in currentComponents)
            {
                var newComponent = new CanvasComponent(
                    Guid.NewGuid(),
                    Id,
                    newVersionId,
                    currentComponent.ComponentType,
                    currentComponent.Content,
                    OwnerId,
                    creationTime
                );

                _components.Add(newComponent);
            }
        }

        public void SwitchToVersion(int versionNumber)
        {
            var targetVersion = _versions.FirstOrDefault(v => v.VersionNumber == versionNumber);
            if (targetVersion == null)
            {
                throw new CanvasVersionNotFoundException(Id, versionNumber);
            }

            // Mark all versions as not current
            foreach (var version in _versions)
            {
                version.SetAsNotCurrent();
            }

            // Mark target version as current
            targetVersion.SetAsCurrent();
            CurrentVersionNumber = versionNumber;
            CurrentVersionId = targetVersion.Id;
        }

        public CanvasComponent GetComponent(CanvasComponentType componentType)
        {
            return _components.FirstOrDefault(c => 
                c.ComponentType == componentType && 
                c.VersionId == CurrentVersionId);
        }

        public void UpdateComponent(
            CanvasComponentType componentType, 
            ComponentContent content, 
            Guid modifiedBy, 
            DateTime modificationTime)
        {
            var component = GetComponent(componentType);
            if (component == null)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:ComponentNotFound")
                    .WithData("componentType", componentType)
                    .WithData("versionId", CurrentVersionId);
            }

            component.UpdateContent(content, modifiedBy, modificationTime);
        }

        public AssociatedHypothesis AddHypothesis(
            CanvasComponentType componentType,
            string assumption,
            Guid createdBy,
            DateTime creationTime)
        {
            var component = GetComponent(componentType);
            if (component == null)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:ComponentNotFound")
                    .WithData("componentType", componentType);
            }

            return component.AddHypothesis(assumption, createdBy, creationTime);
        }

        public void ValidateHypothesis(
            Guid hypothesisId,
            string testDescription,
            string testResult,
            ValidationStatus finalStatus,
            DateTime testStartDate,
            DateTime testEndDate)
        {
            var hypothesis = _components
                .SelectMany(c => c.Hypotheses)
                .FirstOrDefault(h => h.Id == hypothesisId);

            if (hypothesis == null)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:HypothesisNotFound")
                    .WithData("hypothesisId", hypothesisId);
            }

            // Start test first if not already in progress
            if (hypothesis.Details.Status == ValidationStatus.NotTested || 
                hypothesis.Details.Status == ValidationStatus.NeedsMoreTesting)
            {
                hypothesis.StartTest(testDescription, testStartDate);
            }

            // Complete the test
            hypothesis.CompleteTest(testResult, finalStatus, testEndDate);

            // Update component validation state based on hypothesis results
            var component = GetComponent(hypothesis.ComponentType);
            var componentHypotheses = component.Hypotheses.ToList();
            var validatedCount = componentHypotheses.Count(h => h.Details.Status == ValidationStatus.Validated);
            var totalCount = componentHypotheses.Count;
            var confidenceLevel = totalCount > 0 ? (double)validatedCount / totalCount : 0.0;

            ValidationStatus componentStatus;
            if (validatedCount == 0)
                componentStatus = ValidationStatus.NotTested;
            else if (validatedCount == totalCount)
                componentStatus = ValidationStatus.Validated;
            else
                componentStatus = ValidationStatus.InProgress;

            component.UpdateValidationState(new ValidationState(componentStatus, confidenceLevel));

            AddDistributedEvent(new HypothesisValidatedEto
            {
                Id = hypothesisId,
                CanvasId = Id,
                HypothesisId = hypothesisId,
                ComponentType = hypothesis.ComponentType,
                Status = finalStatus,
                TestResult = testResult,
                TenantId = TenantId
            });
        }

        public CanvasVersion GetVersion(int versionNumber)
        {
            return _versions.FirstOrDefault(v => v.VersionNumber == versionNumber);
        }

        public CanvasVersion GetCurrentVersion()
        {
            return _versions.FirstOrDefault(v => v.IsCurrentVersion);
        }

        public IReadOnlyCollection<CanvasComponent> GetComponentsForVersion(int versionNumber)
        {
            var version = GetVersion(versionNumber);
            if (version == null)
            {
                throw new CanvasVersionNotFoundException(Id, versionNumber);
            }

            return _components.Where(c => c.VersionId == version.Id).ToList().AsReadOnly();
        }

        public double GetOverallValidationProgress()
        {
            var currentComponents = _components.Where(c => c.VersionId == CurrentVersionId).ToList();
            if (!currentComponents.Any())
                return 0.0;

            var totalValidated = currentComponents.Count(c => c.ValidationState.Status == ValidationStatus.Validated);
            return (double)totalValidated / currentComponents.Count * 100.0;
        }
    }
}
