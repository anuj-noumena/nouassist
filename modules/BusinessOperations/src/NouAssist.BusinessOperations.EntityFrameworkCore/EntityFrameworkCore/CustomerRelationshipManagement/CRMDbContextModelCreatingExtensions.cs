using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Constants;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.EntityFrameworkCore
{
    public static class CRMDbContextModelCreatingExtensions
    {
        public static void ConfigureCRM(
            this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure Lead aggregate */
            builder.Entity<Lead>(b =>
            {
                b.ToTable(CRMConsts.DbTablePrefix + "Leads", CRMConsts.DbSchema);
                
                b.ConfigureByConvention();
                
                b.Property(x => x.Name).IsRequired().HasMaxLength(CRMConsts.MaxLengths.Name);
                b.Property(x => x.Status).IsRequired();
                b.Property(x => x.Score).IsRequired();
                
                // Configure owned value objects
                b.OwnsOne(x => x.ContactInfo, contactInfo =>
                {
                    contactInfo.Property(c => c.Email).IsRequired().HasMaxLength(CRMConsts.MaxLengths.Email);
                    contactInfo.Property(c => c.Phone).IsRequired().HasMaxLength(CRMConsts.MaxLengths.Phone);
                    contactInfo.Property(c => c.AlternativePhone).HasMaxLength(CRMConsts.MaxLengths.Phone);
                    contactInfo.Property(c => c.Website).HasMaxLength(CRMConsts.MaxLengths.Url);
                    contactInfo.Property(c => c.Address).HasMaxLength(CRMConsts.MaxLengths.Address);
                    contactInfo.Property(c => c.City).HasMaxLength(CRMConsts.MaxLengths.Name);
                    contactInfo.Property(c => c.State).HasMaxLength(CRMConsts.MaxLengths.Name);
                    contactInfo.Property(c => c.PostalCode).HasMaxLength(50);
                    contactInfo.Property(c => c.Country).HasMaxLength(50);
                });
                
                b.OwnsOne(x => x.Details, details =>
                {
                    details.Property(d => d.JobTitle).HasMaxLength(CRMConsts.MaxLengths.Title);
                    details.Property(d => d.CompanyName).HasMaxLength(CRMConsts.MaxLengths.Name);
                    details.Property(d => d.Industry).HasMaxLength(100);
                    details.Property(d => d.Source).IsRequired();
                    details.Property(d => d.ReferralSource).HasMaxLength(CRMConsts.MaxLengths.Name);
                    details.Property(d => d.CampaignId).HasMaxLength(CRMConsts.MaxLengths.Code);
                    details.Property(d => d.Notes).HasMaxLength(CRMConsts.MaxLengths.Note);
                });
                
                // Configure tags as JSON collection
                b.Property(x => x.Tags)
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>())
                    .HasColumnName("Tags");
                
                // Configure relations
                b.HasOne<Contact>().WithOne().HasForeignKey<Contact>(c => c.ConvertedFromLeadId).IsRequired(false);
                
                // Configure index on email for faster lookups and constraints
                b.OwnsOne(x => x.ContactInfo).HasIndex(c => c.Email);
                
                // Configure index on status for faster filtering
                b.HasIndex(x => x.Status);
            });

            /* Configure Contact aggregate */
            builder.Entity<Contact>(b =>
            {
                b.ToTable(CRMConsts.DbTablePrefix + "Contacts", CRMConsts.DbSchema);
                
                b.ConfigureByConvention();
                
                b.Property(x => x.Name).IsRequired().HasMaxLength(CRMConsts.MaxLengths.Name);
                b.Property(x => x.JobTitle).HasMaxLength(CRMConsts.MaxLengths.Title);
                b.Property(x => x.Notes).HasMaxLength(CRMConsts.MaxLengths.Note);
                b.Property(x => x.IsActive).IsRequired();
                b.Property(x => x.IsPrimaryContact).IsRequired();
                
                // Configure owned value objects
                b.OwnsOne(x => x.ContactInfo, contactInfo =>
                {
                    contactInfo.Property(c => c.Email).IsRequired().HasMaxLength(CRMConsts.MaxLengths.Email);
                    contactInfo.Property(c => c.Phone).IsRequired().HasMaxLength(CRMConsts.MaxLengths.Phone);
                    contactInfo.Property(c => c.AlternativePhone).HasMaxLength(CRMConsts.MaxLengths.Phone);
                    contactInfo.Property(c => c.Website).HasMaxLength(CRMConsts.MaxLengths.Url);
                    contactInfo.Property(c => c.Address).HasMaxLength(CRMConsts.MaxLengths.Address);
                    contactInfo.Property(c => c.City).HasMaxLength(CRMConsts.MaxLengths.Name);
                    contactInfo.Property(c => c.State).HasMaxLength(CRMConsts.MaxLengths.Name);
                    contactInfo.Property(c => c.PostalCode).HasMaxLength(50);
                    contactInfo.Property(c => c.Country).HasMaxLength(50);
                });
                
                // Configure tags as JSON collection
                b.Property(x => x.Tags)
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>())
                    .HasColumnName("Tags");
                
                // Configure group IDs as JSON collection
                b.Property(x => x.GroupIds)
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions)null) ?? new List<Guid>())
                    .HasColumnName("GroupIds");
                
                // Configure relations
                b.HasOne<Organization>().WithMany().HasForeignKey(c => c.OrganizationId).IsRequired(false);
                
                // Configure index on email for faster lookups and constraints
                b.OwnsOne(x => x.ContactInfo).HasIndex(c => c.Email);
                
                // Configure index on organization for faster filtering
                b.HasIndex(x => x.OrganizationId);
                
                // Configure index on converted from lead ID
                b.HasIndex(x => x.ConvertedFromLeadId);
            });

            /* Configure Organization aggregate */
            builder.Entity<Organization>(b =>
            {
                b.ToTable(CRMConsts.DbTablePrefix + "Organizations", CRMConsts.DbSchema);
                
                b.ConfigureByConvention();
                
                b.Property(x => x.Name).IsRequired().HasMaxLength(CRMConsts.MaxLengths.Name);
                b.Property(x => x.RegistrationNumber).HasMaxLength(50);
                b.Property(x => x.IsActive).IsRequired();
                
                // Configure owned value objects
                b.OwnsOne(x => x.ContactInfo, contactInfo =>
                {
                    contactInfo.Property(c => c.Email).IsRequired().HasMaxLength(CRMConsts.MaxLengths.Email);
                    contactInfo.Property(c => c.Phone).IsRequired().HasMaxLength(CRMConsts.MaxLengths.Phone);
                    contactInfo.Property(c => c.AlternativePhone).HasMaxLength(CRMConsts.MaxLengths.Phone);
                    contactInfo.Property(c => c.Website).HasMaxLength(CRMConsts.MaxLengths.Url);
                    contactInfo.Property(c => c.Address).HasMaxLength(CRMConsts.MaxLengths.Address);
                    contactInfo.Property(c => c.City).HasMaxLength(CRMConsts.MaxLengths.Name);
                    contactInfo.Property(c => c.State).HasMaxLength(CRMConsts.MaxLengths.Name);
                    contactInfo.Property(c => c.PostalCode).HasMaxLength(50);
                    contactInfo.Property(c => c.Country).HasMaxLength(50);
                });
                
                b.OwnsOne(x => x.Details, details =>
                {
                    details.Property(d => d.Type).IsRequired();
                    details.Property(d => d.Industry).HasMaxLength(100);
                    details.Property(d => d.TaxId).HasMaxLength(50);
                    details.Property(d => d.Notes).HasMaxLength(CRMConsts.MaxLengths.Note);
                });
                
                // Configure tags as JSON collection
                b.Property(x => x.Tags)
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>())
                    .HasColumnName("Tags");
                
                // Configure relations
                b.HasOne<Organization>().WithMany().HasForeignKey(o => o.ParentOrganizationId).IsRequired(false);
                
                // Configure index on name for faster lookups
                b.HasIndex(x => x.Name);
                
                // Configure index on parent organization ID
                b.HasIndex(x => x.ParentOrganizationId);
            });

            /* Configure InteractionLog aggregate */
            builder.Entity<InteractionLog>(b =>
            {
                b.ToTable(CRMConsts.DbTablePrefix + "InteractionLogs", CRMConsts.DbSchema);
                
                b.ConfigureByConvention();
                
                b.Property(x => x.EntityType).IsRequired().HasMaxLength(50);
                b.Property(x => x.EntityId).IsRequired();
                b.Property(x => x.Summary).IsRequired().HasMaxLength(500);
                
                // Configure relations for owned collections
                b.HasMany(x => x.CommunicationRecords)
                    .WithOne()
                    .HasForeignKey(cr => cr.InteractionLogId)
                    .IsRequired();
                
                b.HasMany(x => x.FollowUpTasks)
                    .WithOne()
                    .HasForeignKey(ft => ft.InteractionLogId)
                    .IsRequired();
                
                // Configure index on entity ID and type for faster filtering
                b.HasIndex(x => new { x.EntityId, x.EntityType });
            });
            
            /* Configure CommunicationRecord entity */
            builder.Entity<CommunicationRecord>(b =>
            {
                b.ToTable(CRMConsts.DbTablePrefix + "CommunicationRecords", CRMConsts.DbSchema);
                
                b.ConfigureByConvention();
                
                b.Property(x => x.Type).IsRequired();
                b.Property(x => x.Subject).IsRequired().HasMaxLength(200);
                b.Property(x => x.Content).HasMaxLength(4000);
                b.Property(x => x.CommunicationTime).IsRequired();
                b.Property(x => x.IsInbound).IsRequired();
                b.Property(x => x.Sender).HasMaxLength(256);
                b.Property(x => x.Recipient).HasMaxLength(256);
                
                // Configure attachments as JSON collection
                b.Property(x => x.Attachments)
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<DocumentId>>(v, (JsonSerializerOptions)null) ?? new List<DocumentId>())
                    .HasColumnName("Attachments");
                
                // Configure index on interaction log ID
                b.HasIndex(x => x.InteractionLogId);
                
                // Configure index on communication time for faster filtering
                b.HasIndex(x => x.CommunicationTime);
            });
            
            /* Configure FollowUpTask entity */
            builder.Entity<FollowUpTask>(b =>
            {
                b.ToTable(CRMConsts.DbTablePrefix + "FollowUpTasks", CRMConsts.DbSchema);
                
                b.ConfigureByConvention();
                
                b.Property(x => x.Title).IsRequired().HasMaxLength(200);
                b.Property(x => x.Description).HasMaxLength(1000);
                b.Property(x => x.DueDate).IsRequired();
                b.Property(x => x.Priority).IsRequired();
                b.Property(x => x.Status).IsRequired();
                b.Property(x => x.CompletionNotes).HasMaxLength(1000);
                
                // Configure index on interaction log ID
                b.HasIndex(x => x.InteractionLogId);
                
                // Configure index on status for faster filtering
                b.HasIndex(x => x.Status);
                
                // Configure index on due date for faster filtering
                b.HasIndex(x => x.DueDate);
                
                // Configure index on assigned user for faster filtering
                b.HasIndex(x => x.AssignedUserId);
            });

            /* Configure AutomationWorkflow aggregate */
            builder.Entity<AutomationWorkflow>(b =>
            {
                b.ToTable(CRMConsts.DbTablePrefix + "AutomationWorkflows", CRMConsts.DbSchema);
                
                b.ConfigureByConvention();
                
                b.Property(x => x.Name).IsRequired().HasMaxLength(100);
                b.Property(x => x.Description).HasMaxLength(500);
                b.Property(x => x.IsActive).IsRequired();
                
                // Configure owned value object
                b.OwnsOne(x => x.TriggerCondition, trigger =>
                {
                    trigger.Property(t => t.TriggerType).IsRequired();
                    trigger.Property(t => t.EntityType).IsRequired().HasMaxLength(50);
                    trigger.Property(t => t.FieldName).HasMaxLength(100);
                    trigger.Property(t => t.ExpectedValue).HasMaxLength(100);
                    trigger.Property(t => t.ScheduleExpression).HasMaxLength(100);
                });
                
                // Configure relations for owned collections
                b.HasMany(x => x.Steps)
                    .WithOne()
                    .HasForeignKey(s => s.WorkflowId)
                    .IsRequired();
                
                b.HasMany(x => x.ExecutionRecords)
                    .WithOne()
                    .HasForeignKey(e => e.WorkflowId)
                    .IsRequired();
                
                // Configure index on trigger type for faster filtering
                b.OwnsOne(x => x.TriggerCondition).HasIndex(t => new { t.TriggerType, t.EntityType });
                
                // Configure index on active status
                b.HasIndex(x => x.IsActive);
            });
            
            /* Configure WorkflowStep entity */
            builder.Entity<WorkflowStep>(b =>
            {
                b.ToTable(CRMConsts.DbTablePrefix + "WorkflowSteps", CRMConsts.DbSchema);
                
                b.ConfigureByConvention();
                
                b.Property(x => x.Order).IsRequired();
                b.Property(x => x.Name).IsRequired().HasMaxLength(100);
                b.Property(x => x.Description).HasMaxLength(500);
                b.Property(x => x.ActionType).IsRequired();
                b.Property(x => x.ActionConfiguration).IsRequired();
                b.Property(x => x.IsEnabled).IsRequired();
                
                // Configure index on workflow ID
                b.HasIndex(x => x.WorkflowId);
                
                // Configure index on order for faster sorting
                b.HasIndex(x => new { x.WorkflowId, x.Order });
            });
            
            /* Configure WorkflowExecutionRecord entity */
            builder.Entity<WorkflowExecutionRecord>(b =>
            {
                b.ToTable(CRMConsts.DbTablePrefix + "WorkflowExecutionRecords", CRMConsts.DbSchema);
                
                b.ConfigureByConvention();
                
                b.Property(x => x.ExecutionId).IsRequired();
                b.Property(x => x.TriggeredByEntityId).IsRequired();
                b.Property(x => x.TriggeredByEntityType).IsRequired().HasMaxLength(50);
                b.Property(x => x.StartTime).IsRequired();
                b.Property(x => x.Status).IsRequired().HasMaxLength(50);
                b.Property(x => x.ErrorMessage).HasMaxLength(2000);
                b.Property(x => x.ExecutionLog).HasMaxLength(4000);
                
                // Configure index on workflow ID
                b.HasIndex(x => x.WorkflowId);
                
                // Configure index on execution ID
                b.HasIndex(x => x.ExecutionId).IsUnique();
                
                // Configure index on triggered entity
                b.HasIndex(x => new { x.TriggeredByEntityId, x.TriggeredByEntityType });
                
                // Configure index on status for faster filtering
                b.HasIndex(x => x.Status);
            });

            /* Configure CommunicationTemplate entity */
            builder.Entity<CommunicationTemplate>(b =>
            {
                b.ToTable(CRMConsts.DbTablePrefix + "CommunicationTemplates", CRMConsts.DbSchema);
                
                b.ConfigureByConvention();
                
                b.Property(x => x.Code).IsRequired().HasMaxLength(CRMConsts.MaxLengths.Code);
                b.Property(x => x.Name).IsRequired().HasMaxLength(CRMConsts.MaxLengths.Name);
                b.Property(x => x.Type).IsRequired();
                b.Property(x => x.Subject).IsRequired().HasMaxLength(200);
                b.Property(x => x.Content).IsRequired().HasMaxLength(4000);
                b.Property(x => x.IsActive).IsRequired();
                b.Property(x => x.Category).HasMaxLength(100);
                b.Property(x => x.Description).HasMaxLength(500);
                
                // Configure index on code
                b.HasIndex(x => x.Code).IsUnique();
                
                // Configure index on category for faster filtering
                b.HasIndex(x => x.Category);
                
                // Configure index on type for faster filtering
                b.HasIndex(x => x.Type);
            });
        }
    }
}
