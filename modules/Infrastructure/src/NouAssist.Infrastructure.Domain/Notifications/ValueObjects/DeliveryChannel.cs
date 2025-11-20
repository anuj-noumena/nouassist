using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.Infrastructure.Notifications.ValueObjects
{
    public class DeliveryChannel : ValueObject
    {
        public DeliveryChannelType Type { get; private set; }
        public string Address { get; private set; }
        public bool IsEnabled { get; private set; }
        public int Priority { get; private set; }

        private DeliveryChannel() { }

        public DeliveryChannel(
            DeliveryChannelType type,
            string address,
            bool isEnabled = true,
            int priority = 0)
        {
            Type = type;
            Address = Check.NotNullOrWhiteSpace(address, nameof(address));
            IsEnabled = isEnabled;
            Priority = priority;

            ValidateAddress();
        }

        private void ValidateAddress()
        {
            switch (Type)
            {
                case DeliveryChannelType.Email:
                    if (!Address.Contains("@"))
                    {
                        throw new BusinessException("Notification:InvalidEmailAddress",
                            "Invalid email address for notification delivery.");
                    }
                    break;

                case DeliveryChannelType.SMS:
                    if (Address.Length < 10)
                    {
                        throw new BusinessException("Notification:InvalidPhoneNumber",
                            "Invalid phone number for SMS delivery.");
                    }
                    break;

                case DeliveryChannelType.InApp:
                    // No specific validation for in-app
                    break;

                case DeliveryChannelType.Push:
                    // Could validate device token format
                    break;
            }
        }

        public static DeliveryChannel Email(string emailAddress, bool isEnabled = true)
        {
            return new DeliveryChannel(DeliveryChannelType.Email, emailAddress, isEnabled);
        }

        public static DeliveryChannel SMS(string phoneNumber, bool isEnabled = true)
        {
            return new DeliveryChannel(DeliveryChannelType.SMS, phoneNumber, isEnabled);
        }

        public static DeliveryChannel InApp(string userId, bool isEnabled = true)
        {
            return new DeliveryChannel(DeliveryChannelType.InApp, userId, isEnabled);
        }

        public static DeliveryChannel Push(string deviceToken, bool isEnabled = true)
        {
            return new DeliveryChannel(DeliveryChannelType.Push, deviceToken, isEnabled);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Type;
            yield return Address;
            yield return IsEnabled;
            yield return Priority;
        }
    }

    public enum DeliveryChannelType : byte
    {
        InApp = 1,
        Email = 2,
        SMS = 3,
        Push = 4
    }
}
