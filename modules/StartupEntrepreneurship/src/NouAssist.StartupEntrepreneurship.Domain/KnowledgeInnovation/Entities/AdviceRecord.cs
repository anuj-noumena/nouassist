using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities
{
    /// <summary>
    /// Represents advice given by a mentor
    /// </summary>
    public class AdviceRecord : Entity
    {
        public Guid MentorProfileId { get; private set; }
        public string Topic { get; private set; }
        public string Content { get; private set; }
        public DateTime GivenOn { get; private set; }
        public List<string> Tags { get; private set; }

        protected AdviceRecord()
        {
            Tags = new List<string>();
        }

        internal AdviceRecord(
            Guid mentorProfileId, 
            string topic, 
            string content, 
            DateTime givenOn, 
            List<string> tags = null)
        {
            MentorProfileId = mentorProfileId;
            
            Topic = Check.NotNullOrWhiteSpace(
                topic, 
                nameof(topic), 
                maxLength: KnowledgeInnovationConsts.DefaultLengths.Title);
            
            Content = Check.NotNullOrWhiteSpace(
                content, 
                nameof(content), 
                maxLength: KnowledgeInnovationConsts.KnowledgeAsset.ContentMaxLength);
            
            GivenOn = givenOn;
            Tags = tags ?? new List<string>();
        }

        internal void Update(string topic, string content, List<string> tags)
        {
            Topic = Check.NotNullOrWhiteSpace(
                topic, 
                nameof(topic), 
                maxLength: KnowledgeInnovationConsts.DefaultLengths.Title);
            
            Content = Check.NotNullOrWhiteSpace(
                content, 
                nameof(content), 
                maxLength: KnowledgeInnovationConsts.KnowledgeAsset.ContentMaxLength);
            
            Tags = tags ?? new List<string>();
        }

        public override object[] GetKeys()
        {
            return new object[] { MentorProfileId, Topic, GivenOn };
        }
    }
}
