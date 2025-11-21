using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects
{
    /// <summary>
    /// Value object representing communication preferences for an investor
    /// </summary>
    public class CommunicationPreferences : ValueObject
    {
        public CommunicationChannel PreferredChannel { get; private set; }
        public UpdateFrequency UpdateFrequency { get; private set; }
        public bool AllowMarketing { get; private set; }
        public List<string> Tags { get; private set; }

        private CommunicationPreferences() 
        {
            Tags = new List<string>();
        }

        public CommunicationPreferences(
            CommunicationChannel preferredChannel,
            UpdateFrequency updateFrequency,
            bool allowMarketing = true,
            List<string> tags = null)
        {
            PreferredChannel = preferredChannel;
            UpdateFrequency = updateFrequency;
            AllowMarketing = allowMarketing;
            Tags = tags ?? new List<string>();
        }

        public CommunicationPreferences UpdatePreferredChannel(CommunicationChannel channel)
        {
            return new CommunicationPreferences(channel, UpdateFrequency, AllowMarketing, Tags);
        }

        public CommunicationPreferences UpdateFrequency(UpdateFrequency frequency)
        {
            return new CommunicationPreferences(PreferredChannel, frequency, AllowMarketing, Tags);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PreferredChannel;
            yield return UpdateFrequency;
            yield return AllowMarketing;
            foreach (var tag in Tags.OrderBy(t => t))
            {
                yield return tag;
            }
        }
    }
}
