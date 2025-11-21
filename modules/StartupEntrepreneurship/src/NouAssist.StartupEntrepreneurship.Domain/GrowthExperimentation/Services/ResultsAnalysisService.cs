using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Exceptions;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Repositories;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services.Interfaces;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services
{
    /// <summary>
    /// Implementation of the results analysis service
    /// </summary>
    public class ResultsAnalysisService : DomainService, IResultsAnalysisService
    {
        private readonly IGrowthExperimentRepository _experimentRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<ResultsAnalysisService> _logger;

        public ResultsAnalysisService(
            IGrowthExperimentRepository experimentRepository,
            IGuidGenerator guidGenerator,
            ILogger<ResultsAnalysisService> logger)
        {
            _experimentRepository = experimentRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task RecordResultDataAsync(
            Guid variantId,
            string metricName,
            MetricType metricType,
            double value,
            int sampleSize,
            double? standardDeviation = null,
            double? marginOfError = null,
            bool isPrimary = false)
        {
            try
            {
                // Create result value object
                var resultValue = new ResultValue(
                    metricType, 
                    value, 
                    sampleSize, 
                    standardDeviation, 
                    marginOfError);
                
                // Check if result data already exists for this variant and metric
                var existingResults = await _experimentRepository.GetResultsForVariantAsync(variantId);
                var existingResult = existingResults.FirstOrDefault(r => r.MetricName == metricName);
                
                if (existingResult != null)
                {
                    // Update existing result
                    existingResult.UpdateValue(resultValue, Clock.Now);
                    await _experimentRepository.UpdateResultDataAsync(existingResult);
                    
                    _logger.LogInformation(
                        "Updated result data for variant {VariantId}, metric {MetricName}",
                        variantId,
                        metricName);
                }
                else
                {
                    // Create new result
                    var resultData = new ExperimentResultData(
                        _guidGenerator.Create(),
                        variantId,
                        metricName,
                        metricType,
                        resultValue,
                        isPrimary,
                        Clock.Now);
                    
                    await _experimentRepository.AddResultDataAsync(resultData);
                    
                    _logger.LogInformation(
                        "Recorded new result data for variant {VariantId}, metric {MetricName}",
                        variantId,
                        metricName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to record result data for variant {VariantId}, metric {MetricName}",
                    variantId,
                    metricName);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<StatisticalSignificance> AnalyzeSignificanceAsync(
            Guid experimentId,
            string primaryMetricName,
            double confidenceLevel = GrowthExperimentationConstants.Experiments.DefaultSignificanceLevel)
        {
            try
            {
                var experiment = await _experimentRepository.GetWithDetailsAsync(experimentId);
                
                if (experiment == null)
                {
                    throw new ExperimentNotFoundException(experimentId);
                }
                
                // Get control variant
                var controlVariant = experiment.Variants.FirstOrDefault(v => v.Type == VariantType.Control);
                if (controlVariant == null)
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:NoControlVariant");
                }
                
                // Get control variant results
                var controlResults = await _experimentRepository.GetResultsForVariantAsync(controlVariant.Id);
                var controlPrimaryResult = controlResults.FirstOrDefault(r => r.MetricName == primaryMetricName && r.IsPrimary);
                
                if (controlPrimaryResult == null)
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:NoPrimaryMetricData")
                        .WithData("variantId", controlVariant.Id)
                        .WithData("metricName", primaryMetricName);
                }
                
                // Get treatment variants and their results
                var treatmentVariants = experiment.Variants.Where(v => v.Type == VariantType.Treatment).ToList();
                if (!treatmentVariants.Any())
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:NoTreatmentVariants");
                }
                
                // Simple implementation for calculating p-value
                // In a real application, this would use proper statistical methods
                // like t-test or chi-square test depending on the metric type
                double pValue = CalculatePValue(controlPrimaryResult, treatmentVariants, primaryMetricName);
                
                int totalSampleSize = controlPrimaryResult.Value.SampleSize;
                
                // Create significance object
                var significance = new StatisticalSignificance(
                    pValue,
                    confidenceLevel,
                    totalSampleSize);
                
                _logger.LogInformation(
                    "Analyzed significance for experiment {ExperimentId}, p-value: {PValue}, significant: {IsSignificant}",
                    experimentId,
                    pValue,
                    significance.IsSignificant);
                
                return significance;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to analyze significance for experiment {ExperimentId}",
                    experimentId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<Guid?> DetermineWinningVariantAsync(
            Guid experimentId,
            string primaryMetricName)
        {
            try
            {
                var experiment = await _experimentRepository.GetWithDetailsAsync(experimentId);
                
                if (experiment == null)
                {
                    throw new ExperimentNotFoundException(experimentId);
                }
                
                // Get all variants
                var variants = experiment.Variants.ToList();
                
                // Get results for each variant
                var variantResults = new Dictionary<Guid, ExperimentResultData>();
                
                foreach (var variant in variants)
                {
                    var results = await _experimentRepository.GetResultsForVariantAsync(variant.Id);
                    var primaryResult = results.FirstOrDefault(r => r.MetricName == primaryMetricName && r.IsPrimary);
                    
                    if (primaryResult != null)
                    {
                        variantResults.Add(variant.Id, primaryResult);
                    }
                }
                
                if (variantResults.Count < variants.Count)
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:IncompleteResults");
                }
                
                // Determine winning variant based on highest metric value
                // Note: This logic would change based on the type of metric
                // For some metrics, lower is better (e.g., cost, bounce rate)
                Guid? winningVariantId = null;
                double bestValue = 0;
                
                foreach (var result in variantResults)
                {
                    var variant = variants.First(v => v.Id == result.Key);
                    var metricValue = result.Value.Value.Value;
                    
                    // For simplicity, assuming higher is always better
                    // In a real implementation, would check metric type
                    if (winningVariantId == null || metricValue > bestValue)
                    {
                        winningVariantId = variant.Id;
                        bestValue = metricValue;
                    }
                }
                
                _logger.LogInformation(
                    "Determined winning variant {VariantId} for experiment {ExperimentId}",
                    winningVariantId,
                    experimentId);
                
                return winningVariantId;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to determine winning variant for experiment {ExperimentId}",
                    experimentId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task CompleteExperimentAnalysisAsync(
            Guid experimentId,
            ExperimentLearnings learnings)
        {
            try
            {
                var experiment = await _experimentRepository.GetAsync(experimentId);
                
                if (experiment == null)
                {
                    throw new ExperimentNotFoundException(experimentId);
                }
                
                if (experiment.Status != ExperimentStatus.AnalysisInProgress)
                {
                    throw new ExperimentInvalidStateException(
                        experimentId, 
                        experiment.Status, 
                        ExperimentStatus.AnalysisInProgress);
                }
                
                // Ensure significance has been calculated
                if (experiment.Significance == null)
                {
                    // Calculate significance if not already done
                    var targetMetric = experiment.TargetMetrics.FirstOrDefault(m => m.IsPrimary);
                    
                    if (targetMetric == null)
                    {
                        throw new BusinessException("GrowthExperimentation:ErrorMessage:NoPrimaryMetricDefined");
                    }
                    
                    var significance = await AnalyzeSignificanceAsync(experimentId, targetMetric.Name);
                    var winningVariantId = await DetermineWinningVariantAsync(experimentId, targetMetric.Name);
                    
                    experiment.SetSignificance(significance, winningVariantId);
                    await _experimentRepository.UpdateAsync(experiment);
                }
                
                // Complete the experiment with learnings
                experiment.FinalizeAnalysis(learnings);
                await _experimentRepository.UpdateAsync(experiment);
                
                _logger.LogInformation(
                    "Completed analysis for experiment {ExperimentId}",
                    experimentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to complete analysis for experiment {ExperimentId}",
                    experimentId);
                throw;
            }
        }

        private double CalculatePValue(
            ExperimentResultData controlResult, 
            List<ExperimentVariant> treatmentVariants, 
            string metricName)
        {
            // This is a simplified placeholder implementation
            // In a real application, use proper statistical methods based on the metric type
            
            // For now, returning a random p-value for demonstration
            // In production, this would use proper statistical calculations
            var random = new Random();
            return random.NextDouble() * 0.2; // Generate p-value between 0 and 0.2
        }
    }
}
