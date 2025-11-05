using System;
using System.Collections.Generic;
using NouAssist.Contacts.Aggregates;
using Shouldly;
using Xunit;

namespace NouAssist.Contacts.Services
{
    public class ContactRelationshipServiceTests
    {
        private readonly ContactRelationshipService _service;

        public ContactRelationshipServiceTests()
        {
            _service = new ContactRelationshipService();
        }

        [Fact]
        public void CalculateRelationshipStrength_Should_ReturnHighScore_ForRecentContact()
        {
            // Arrange
            var contact = new Contact(
                Guid.NewGuid(),
                Guid.NewGuid(),
                firstName: "John",
                lastName: "Doe");
            contact.RecordContact(); // Just contacted
            contact.Importance = 8;

            // Act
            var strength = _service.CalculateRelationshipStrength(contact);

            // Assert
            strength.OverallScore.ShouldBeGreaterThan(70);
            strength.Category.ShouldBe(RelationshipCategory.Strong);
        }

        [Fact]
        public void SuggestContactsToReach_Should_ReturnContactsNeedingAttention()
        {
            // Arrange
            var contact1 = new Contact(
                Guid.NewGuid(),
                Guid.NewGuid(),
                firstName: "John",
                lastName: "Doe");
            contact1.Importance = 9;
            contact1.ScheduleFollowup(DateTime.Today.AddDays(-1)); // Overdue

            var contact2 = new Contact(
                Guid.NewGuid(),
                Guid.NewGuid(),
                firstName: "Jane",
                lastName: "Smith");
            contact2.RecordContact(); // Recently contacted

            var contacts = new List<Contact> { contact1, contact2 };

            // Act
            var recommendations = _service.SuggestContactsToReach(contacts, 5);

            // Assert
            recommendations.ShouldNotBeEmpty();
            recommendations[0].ContactId.ShouldBe(contact1.Id);
        }
    }
}
