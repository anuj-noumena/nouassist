using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using NouAssist.WorkflowOrchestration.WorkflowEngine.Aggregates;

 

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.EntityFrameworkCore.Configurations

{

    public class TransitionPermissionPolicyConfiguration : IEntityTypeConfiguration<TransitionPermissionPolicy>

    {

        public void Configure(EntityTypeBuilder<TransitionPermissionPolicy> builder)

        {

            builder.ToTable(

                WorkflowDbProperties.DbTablePrefix + "TransitionPermissionPolicies",

                WorkflowDbProperties.DbSchema);

 

            builder.HasKey(x => x.Id);

 

            // Scoping properties

            builder.Property(x => x.Scope)

                .IsRequired();

 

            builder.Property(x => x.ProjectId)

                .IsRequired(false); // Nullable for global policies

 

            builder.Property(x => x.WorkflowId)

                .IsRequired();

 

            builder.Property(x => x.TransitionId)

                .IsRequired();

 

            // Metadata

            builder.Property(x => x.Name)

                .IsRequired()

                .HasMaxLength(256);

 

            builder.Property(x => x.Description)

                .HasMaxLength(2000);

 

            builder.Property(x => x.IsActive)

                .IsRequired()

                .HasDefaultValue(true);

 

            builder.Property(x => x.Priority)

                .IsRequired()

                .HasDefaultValue(0);

 

            // Collections stored as JSON

            builder.OwnsMany(x => x.AllowedRoles, rolesBuilder =>

            {

                rolesBuilder.ToJson();

            });

 

            builder.OwnsMany(x => x.AllowedUserIds, userIdsBuilder =>

            {

                userIdsBuilder.ToJson();

            });

 

            // Indexes for efficient querying

            // Most common query: Get policy for specific transition and project

            builder.HasIndex(x => new { x.TransitionId, x.ProjectId, x.IsActive })

                .HasDatabaseName("IX_TransitionPermissionPolicies_Transition_Project_Active");

 

            // Query by workflow

            builder.HasIndex(x => new { x.WorkflowId, x.IsActive })

                .HasDatabaseName("IX_TransitionPermissionPolicies_Workflow_Active");

 

            // Query by project

            builder.HasIndex(x => new { x.ProjectId, x.IsActive })

                .HasDatabaseName("IX_TransitionPermissionPolicies_Project_Active");

 

            // Scope and transition for filtering

            builder.HasIndex(x => new { x.Scope, x.TransitionId, x.IsActive })

                .HasDatabaseName("IX_TransitionPermissionPolicies_Scope_Transition_Active");

 

            // Priority sorting within transition context

            builder.HasIndex(x => new { x.TransitionId, x.Priority })

                .HasDatabaseName("IX_TransitionPermissionPolicies_Transition_Priority");

        }

    }

}