namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Mapping
{
    public class CollaborationApplicationAutoMapperProfile : Profile
    {
        public CollaborationApplicationAutoMapperProfile()
        {
            // Access Control mappings
            CreateMap<UserAccessProfile, UserAccessProfileDto>()
                .ForMember(d => d.UserName, opt => opt.Ignore()); // This will be populated from external service

            CreateMap<Role, RoleDto>();

            CreateMap<Permission, PermissionDto>()
                .ForMember(d => d.ResourceType, opt => opt.MapFrom(s => s.Resource.Type))
                .ForMember(d => d.ResourceId, opt => opt.MapFrom(s => s.Resource.Id))
                .ForMember(d => d.ResourceDisplayName, opt => opt.Ignore()); // Populated in app service

            // Notification mappings
            CreateMap<NotificationConfiguration, NotificationConfigurationDto>()
                .ForMember(d => d.UserName, opt => opt.Ignore()); // This will be populated from external service

            CreateMap<SubscriptionRule, SubscriptionRuleDto>()
                .ForMember(d => d.EventType, opt => opt.MapFrom(s => s.EventTrigger.EventType))
                .ForMember(d => d.ResourceType, opt => opt.MapFrom(s => s.EventTrigger.TargetResource.Type))
                .ForMember(d => d.ResourceId, opt => opt.MapFrom(s => s.EventTrigger.TargetResource.Id))
                .ForMember(d => d.ResourceDisplayName, opt => opt.Ignore()); // Populated in app service

            // Discussion mappings
            CreateMap<DiscussionBoard, DiscussionBoardDto>()
                .ForMember(d => d.ResourceType, opt => opt.MapFrom(s => s.Resource.Type))
                .ForMember(d => d.ResourceId, opt => opt.MapFrom(s => s.Resource.Id))
                .ForMember(d => d.ResourceDisplayName, opt => opt.Ignore()) // Populated in app service
                .ForMember(d => d.ThreadCount, opt => opt.MapFrom(s => s.Threads.Count))
                .ForMember(d => d.CommentCount, opt => opt.MapFrom(s => s.Threads.Sum(t => t.Comments.Count)));

            CreateMap<DiscussionThread, DiscussionThreadDto>()
                .ForMember(d => d.Subject, opt => opt.MapFrom(s => s.Subject.Value))
                .ForMember(d => d.CreatorUserName, opt => opt.Ignore()); // This will be populated from external service

            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.Content, opt => opt.MapFrom(s => s.Content.Value))
                .ForMember(d => d.MentionedUserIds, opt => opt.MapFrom(s => s.Content.MentionedUserIds))
                .ForMember(d => d.AuthorUserName, opt => opt.Ignore()) // This will be populated from external service
                .ForMember(d => d.MentionedUserNames, opt => opt.Ignore()); // This will be populated from external service
        }
    }
}
