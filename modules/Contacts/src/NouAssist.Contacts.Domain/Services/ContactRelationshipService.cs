using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Contacts.Aggregates;
using Volo.Abp.Domain.Services;

namespace NouAssist.Contacts.Services
{
    /// <summary>
    /// Domain service for managing contact relationships and networks
    /// </summary>
    public class ContactRelationshipService : DomainService
    {
        public RelationshipStrength CalculateRelationshipStrength(Contact contact)
        {
            var strength = new RelationshipStrength
            {
                ContactId = contact.Id,
                OverallScore = 0
            };

            // Factor 1: Recency of contact
            if (contact.LastContactedAt.HasValue)
            {
                var daysSinceContact = (Clock.Now - contact.LastContactedAt.Value).Days;
                strength.RecencyScore = CalculateRecencyScore(daysSinceContact);
            }

            // Factor 2: Frequency of contact
            strength.FrequencyScore = contact.ContactFrequencyDays <= 7 ? 100 : 
                                      contact.ContactFrequencyDays <= 30 ? 70 : 30;

            // Factor 3: Importance rating
            strength.ImportanceScore = (int)(contact.Importance * 10);

            // Factor 4: Meeting history
            if (contact.LastMetAt.HasValue)
            {
                var daysSinceMeeting = (Clock.Now - contact.LastMetAt.Value).Days;
                strength.MeetingScore = daysSinceMeeting < 30 ? 50 : daysSinceMeeting < 90 ? 30 : 10;
            }

            // Calculate overall score
            strength.OverallScore = (int)(
                strength.RecencyScore * 0.3 +
                strength.FrequencyScore * 0.3 +
                strength.ImportanceScore * 0.2 +
                strength.MeetingScore * 0.2);

            strength.Category = strength.OverallScore >= 80 ? RelationshipCategory.Strong :
                               strength.OverallScore >= 50 ? RelationshipCategory.Moderate :
                               RelationshipCategory.Weak;

            return strength;
        }

        public List<ContactRecommendation> SuggestContactsToReach(
            List<Contact> contacts,
            int maxSuggestions = 5)
        {
            var recommendations = new List<ContactRecommendation>();

            foreach (var contact in contacts.Where(c => c.IsActive))
            {
                var score = 0;
                var reasons = new List<string>();

                // High importance contacts
                if (contact.Importance >= 8)
                {
                    score += 30;
                    reasons.Add("High importance contact");
                }

                // Haven't contacted recently
                if (contact.LastContactedAt.HasValue)
                {
                    var daysSince = (Clock.Now - contact.LastContactedAt.Value).Days;
                    if (daysSince > contact.ContactFrequencyDays)
                    {
                        score += 40;
                        reasons.Add($"Last contacted {daysSince} days ago");
                    }
                }

                // Pending follow-up
                if (contact.NeedsFollowup)
                {
                    score += 50;
                    reasons.Add("Follow-up scheduled");
                }

                // No recent contact at all
                if (!contact.LastContactedAt.HasValue || 
                    (Clock.Now - contact.LastContactedAt.Value).Days > 90)
                {
                    score += 20;
                    reasons.Add("No recent contact");
                }

                if (score > 0)
                {
                    recommendations.Add(new ContactRecommendation
                    {
                        ContactId = contact.Id,
                        ContactName = contact.DisplayName,
                        RecommendationScore = score,
                        Reasons = reasons
                    });
                }
            }

            return recommendations
                .OrderByDescending(r => r.RecommendationScore)
                .Take(maxSuggestions)
                .ToList();
        }

        public ContactNetworkAnalysis AnalyzeContactNetwork(List<Contact> contacts, List<Client> clients)
        {
            var analysis = new ContactNetworkAnalysis
            {
                TotalContacts = contacts.Count,
                ActiveContacts = contacts.Count(c => c.IsActive),
                TotalClients = clients.Count,
                ActiveClients = clients.Count(c => c.IsActive)
            };

            // Analyze by relationship type
            analysis.ByRelationship = contacts
                .GroupBy(c => c.Relationship)
                .ToDictionary(g => g.Key.ToString(), g => g.Count());

            // Analyze by contact type
            analysis.ByType = contacts
                .GroupBy(c => c.ContactType)
                .ToDictionary(g => g.Key.ToString(), g => g.Count());

            // Identify top contacts by importance
            analysis.TopContacts = contacts
                .OrderByDescending(c => c.Importance)
                .Take(10)
                .Select(c => new ContactSummary
                {
                    Id = c.Id,
                    Name = c.DisplayName,
                    Importance = c.Importance,
                    LastContactedDaysAgo = c.LastContactedAt.HasValue ? 
                        (Clock.Now - c.LastContactedAt.Value).Days : (int?)null
                })
                .ToList();

            // Identify contacts needing attention
            analysis.NeedingAttention = contacts
                .Where(c => c.NeedsFollowup || 
                           !c.LastContactedAt.HasValue ||
                           (Clock.Now - c.LastContactedAt.Value).Days > c.ContactFrequencyDays)
                .Select(c => c.Id)
                .ToList();

            return analysis;
        }

        private int CalculateRecencyScore(int daysSinceContact)
        {
            if (daysSinceContact <= 7) return 100;
            if (daysSinceContact <= 30) return 80;
            if (daysSinceContact <= 90) return 50;
            if (daysSinceContact <= 180) return 30;
            return 10;
        }
    }

    public class RelationshipStrength
    {
        public Guid ContactId { get; set; }
        public int RecencyScore { get; set; }
        public int FrequencyScore { get; set; }
        public int ImportanceScore { get; set; }
        public int MeetingScore { get; set; }
        public int OverallScore { get; set; }
        public RelationshipCategory Category { get; set; }
    }

    public class ContactRecommendation
    {
        public Guid ContactId { get; set; }
        public string ContactName { get; set; }
        public int RecommendationScore { get; set; }
        public List<string> Reasons { get; set; } = new();
    }

    public class ContactNetworkAnalysis
    {
        public int TotalContacts { get; set; }
        public int ActiveContacts { get; set; }
        public int TotalClients { get; set; }
        public int ActiveClients { get; set; }
        public Dictionary<string, int> ByRelationship { get; set; } = new();
        public Dictionary<string, int> ByType { get; set; } = new();
        public List<ContactSummary> TopContacts { get; set; } = new();
        public List<Guid> NeedingAttention { get; set; } = new();
    }

    public class ContactSummary
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Importance { get; set; }
        public int? LastContactedDaysAgo { get; set; }
    }

    public enum RelationshipCategory
    {
        Strong,
        Moderate,
        Weak,
        Dormant
    }
}
