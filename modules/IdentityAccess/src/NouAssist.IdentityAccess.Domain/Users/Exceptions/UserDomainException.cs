using System;
using Volo.Abp;

namespace NouAssist.IdentityAccess.Users.Exceptions
{
    public class UserDomainException : BusinessException
    {
        public UserDomainException(string code, string? message = null, string? details = null, Exception? innerException = null)
            : base(code, message, details, innerException)
        {
        }

        public static UserDomainException EmailRequired()
        {
            return new UserDomainException(
                "Users:EmailRequired",
                "Email address is required.");
        }

        public static UserDomainException InvalidEmailFormat(string email)
        {
            return new UserDomainException(
                "Users:InvalidEmailFormat",
                $"Email address '{email}' is not in a valid format.");
        }

        public static UserDomainException UserNotActive(Guid userId)
        {
            return new UserDomainException(
                "Users:UserNotActive",
                $"User {userId} is not active.");
        }

        public static UserDomainException ProfileIncomplete()
        {
            return new UserDomainException(
                "Users:ProfileIncomplete",
                "User profile is incomplete. Please provide required information.");
        }

        public static UserDomainException FirstNameTooLong(int maxLength)
        {
            return new UserDomainException(
                "Users:FirstNameTooLong",
                $"First name cannot exceed {maxLength} characters.");
        }

        public static UserDomainException LastNameTooLong(int maxLength)
        {
            return new UserDomainException(
                "Users:LastNameTooLong",
                $"Last name cannot exceed {maxLength} characters.");
        }

        public static UserDomainException BioTooLong(int maxLength)
        {
            return new UserDomainException(
                "Users:BioTooLong",
                $"Bio cannot exceed {maxLength} characters.");
        }
    }
}
