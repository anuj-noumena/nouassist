using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceBooking;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Specifications.Base;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Specifications.ResourceBooking
{
    /// <summary>
    /// Specifications for resource bookings
    /// </summary>
    public static class ResourceBookingSpecifications
    {
        /// <summary>
        /// Specification for active bookings
        /// </summary>
        public class ActiveBookingSpecification : Specification<ResourceBookingAggregate>
        {
            public override Expression<Func<ResourceBookingAggregate, bool>> ToExpression()
            {
                return booking => booking.Status == BookingStatus.Confirmed || booking.Status == BookingStatus.InUse;
            }
        }

        /// <summary>
        /// Specification for bookings during a specific time period
        /// </summary>
        public class BookingDuringTimeRangeSpecification : Specification<ResourceBookingAggregate>
        {
            private readonly DateTime _startTime;
            private readonly DateTime _endTime;

            public BookingDuringTimeRangeSpecification(DateTime startTime, DateTime endTime)
            {
                _startTime = startTime;
                _endTime = endTime;
            }

            public override Expression<Func<ResourceBookingAggregate, bool>> ToExpression()
            {
                return booking => 
                    booking.TimeSlot.StartTime < _endTime && booking.TimeSlot.EndTime > _startTime &&
                    (booking.Status == BookingStatus.Confirmed || booking.Status == BookingStatus.InUse);
            }
        }

        /// <summary>
        /// Specification for bookings of a specific resource
        /// </summary>
        public class ResourceBookingSpecification : Specification<ResourceBookingAggregate>
        {
            private readonly Guid _resourceInstanceId;

            public ResourceBookingSpecification(Guid resourceInstanceId)
            {
                _resourceInstanceId = resourceInstanceId;
            }

            public override Expression<Func<ResourceBookingAggregate, bool>> ToExpression()
            {
                return booking => booking.ResourceInstanceId == _resourceInstanceId;
            }
        }

        /// <summary>
        /// Specification for bookings by a specific user
        /// </summary>
        public class UserBookingSpecification : Specification<ResourceBookingAggregate>
        {
            private readonly Guid _userId;
            private readonly bool _includeAssignedToUser;

            public UserBookingSpecification(Guid userId, bool includeAssignedToUser = false)
            {
                _userId = userId;
                _includeAssignedToUser = includeAssignedToUser;
            }

            public override Expression<Func<ResourceBookingAggregate, bool>> ToExpression()
            {
                if (_includeAssignedToUser)
                {
                    return booking => booking.BookedByUserId == _userId || booking.AssignedToUserId == _userId;
                }

                return booking => booking.BookedByUserId == _userId;
            }
        }

        /// <summary>
        /// Specification for bookings with a specific status
        /// </summary>
        public class BookingStatusSpecification : Specification<ResourceBookingAggregate>
        {
            private readonly BookingStatus _status;

            public BookingStatusSpecification(BookingStatus status)
            {
                _status = status;
            }

            public override Expression<Func<ResourceBookingAggregate, bool>> ToExpression()
            {
                return booking => booking.Status == _status;
            }
        }

        /// <summary>
        /// Specification for bookings for a specific related entity
        /// </summary>
        public class RelatedEntityBookingSpecification : Specification<ResourceBookingAggregate>
        {
            private readonly string _relatedEntity;
            private readonly Guid _relatedEntityId;

            public RelatedEntityBookingSpecification(string relatedEntity, Guid relatedEntityId)
            {
                _relatedEntity = relatedEntity;
                _relatedEntityId = relatedEntityId;
            }

            public override Expression<Func<ResourceBookingAggregate, bool>> ToExpression()
            {
                return booking => 
                    booking.RelatedEntity == _relatedEntity && 
                    booking.RelatedEntityId == _relatedEntityId;
            }
        }
    }
}
