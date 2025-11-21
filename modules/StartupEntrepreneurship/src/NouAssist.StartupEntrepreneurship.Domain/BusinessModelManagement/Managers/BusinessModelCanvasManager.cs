using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Exceptions;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Repositories;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Specifications;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Managers
{
    /// <summary>
    /// Manager for BusinessModelCanvas aggregate - handles complex business logic
    /// </summary>
    public class BusinessModelCanvasManager : DomainService
    {
        private readonly IBusinessModelCanvasRepository _canvasRepository;
        private readonly ILogger<BusinessModelCanvasManager> _logger;

        public BusinessModelCanvasManager(
            IBusinessModelCanvasRepository canvasRepository,
            ILogger<BusinessModelCanvasManager> logger)
        {
            _canvasRepository = canvasRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new Business Model Canvas with validation
        /// </summary>
        public async Task<BusinessModelCanvas> CreateAsync(
            string title,
            string description,
            Guid ownerId)
        {
            // Check for duplicate title within same tenant
            var existingCanvas = await _canvasRepository.FindByTitleAsync(
                title,
                CurrentTenant.Id);

            if (existingCanvas != null)
            {
                throw new BusinessModelManagementDomainException(
                    "BusinessModelManagement:DuplicateTitle",
                    $"A Business Model Canvas with title '{title}' already exists.");
            }

            var canvas = BusinessModelCanvas.Create(
                GuidGenerator.Create(),
                title,
                description,
                ownerId,
                CurrentTenant.Id,
                Clock.Now);

            _logger.LogInformation(
                "Created new Business Model Canvas {CanvasId} with title '{Title}'",
                canvas.Id, canvas.Title);

            return canvas;
        }

        /// <summary>
        /// Updates canvas content with validation
        /// </summary>
        public async Task<BusinessModelCanvas> UpdateAsync(
            Guid canvasId,
            string? newTitle = null,
            string? newDescription = null)
        {
            var canvas = await _canvasRepository.GetAsync(canvasId);

            // Check for duplicate title if title is being changed
            if (!string.IsNullOrWhiteSpace(newTitle) && canvas.Title != newTitle)
            {
                var existingCanvas = await _canvasRepository.FindByTitleAsync(
                    newTitle,
                    CurrentTenant.Id);

                if (existingCanvas != null && existingCanvas.Id != canvasId)
                {
                    throw new BusinessModelManagementDomainException(
                        "BusinessModelManagement:DuplicateTitle",
                        $"A Business Model Canvas with title '{newTitle}' already exists.");
                }

                canvas.UpdateTitle(newTitle);
            }

            if (!string.IsNullOrWhiteSpace(newDescription))
            {
                canvas.UpdateDescription(newDescription);
            }

            await _canvasRepository.UpdateAsync(canvas);

            _logger.LogInformation(
                "Updated Business Model Canvas {CanvasId}",
                canvasId);

            return canvas;
        }

        /// <summary>
        /// Deletes a canvas (soft delete)
        /// </summary>
        public async Task DeleteAsync(Guid canvasId, bool hardDelete = false)
        {
            var canvas = await _canvasRepository.GetAsync(canvasId);

            if (hardDelete)
            {
                await _canvasRepository.DeleteAsync(canvas);
                _logger.LogWarning("Hard deleted canvas {CanvasId}", canvasId);
            }
            else
            {
                // Soft delete is handled by ABP framework
                await _canvasRepository.DeleteAsync(canvas, autoSave: true);
                _logger.LogInformation("Soft deleted canvas {CanvasId}", canvasId);
            }
        }

        /// <summary>
        /// Creates a new version of the canvas
        /// </summary>
        public async Task<BusinessModelCanvas> CreateVersionAsync(
            Guid canvasId,
            string versionName,
            string notes,
            Guid createdBy)
        {
            var canvas = await _canvasRepository.GetWithVersionsAsync(canvasId);

            // Check for duplicate version name
            if (canvas.Versions.Any(v => v.VersionName == versionName))
            {
                throw new DuplicateVersionException(
                    $"A version with name '{versionName}' already exists.");
            }

            canvas.CreateNewVersion(versionName, notes, createdBy, Clock.Now);

            await _canvasRepository.UpdateAsync(canvas);

            _logger.LogInformation(
                "Created new version '{VersionName}' for canvas {CanvasId}",
                versionName, canvasId);

            return canvas;
        }

        /// <summary>
        /// Validates if a canvas can be pivoted based on business rules
        /// </summary>
        public bool CanPivot(BusinessModelCanvas canvas)
        {
            // Business rule: Canvas must have at least one version
            if (canvas.CurrentVersionNumber < 1)
            {
                return false;
            }

            // Add more business rules as needed
            return true;
        }
    }
}
