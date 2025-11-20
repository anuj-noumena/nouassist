using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities
{
    /// <summary>
    /// Represents metadata about a prediction model
    /// </summary>
    public class PredictionModelMetadata : Entity
    {
        /// <summary>
        /// Name of the model
        /// </summary>
        public string ModelName { get; protected set; }
        
        /// <summary>
        /// Version of the model
        /// </summary>
        public string ModelVersion { get; protected set; }
        
        /// <summary>
        /// Features used in the model (comma-separated)
        /// </summary>
        public string Features { get; protected set; }
        
        /// <summary>
        /// Feature importance mapping
        /// </summary>
        private readonly Dictionary<string, double> _featureImportance;
        
        /// <summary>
        /// Public access to feature importance
        /// </summary>
        public IReadOnlyDictionary<string, double> FeatureImportance => _featureImportance;

        protected PredictionModelMetadata()
        {
            _featureImportance = new Dictionary<string, double>();
        }

        /// <summary>
        /// Creates new prediction model metadata
        /// </summary>
        public PredictionModelMetadata(
            string modelName,
            string modelVersion,
            string features)
        {
            ModelName = modelName;
            ModelVersion = modelVersion;
            Features = features;
            _featureImportance = new Dictionary<string, double>();
        }

        /// <summary>
        /// Adds feature importance data
        /// </summary>
        public void AddFeatureImportance(string feature, double importance)
        {
            _featureImportance[feature] = importance;
        }

        /// <summary>
        /// Gets the entity keys
        /// </summary>
        public override object[] GetKeys()
        {
            return new object[] { ModelName, ModelVersion };
        }
    }
}
