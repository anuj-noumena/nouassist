using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.SharedKernel.ValueObjects
{
    public class EventLocation : ValueObject
    {
        public string? Name { get; private set; }
        public string? Address { get; private set; }
        public string? City { get; private set; }
        public string? Country { get; private set; }
        public double? Latitude { get; private set; }
        public double? Longitude { get; private set; }
        public string? VirtualMeetingUrl { get; private set; }

        private EventLocation() { }

        public EventLocation(
            string? name = null,
            string? address = null,
            string? city = null,
            string? country = null,
            double? latitude = null,
            double? longitude = null,
            string? virtualMeetingUrl = null)
        {
            Name = name;
            Address = address;
            City = city;
            Country = country;
            Latitude = latitude;
            Longitude = longitude;
            VirtualMeetingUrl = virtualMeetingUrl;
        }

        public static EventLocation Physical(string name, string address, string city) =>
            new(name: name, address: address, city: city);

        public static EventLocation Virtual(string meetingUrl) =>
            new(virtualMeetingUrl: meetingUrl);

        public bool IsVirtual => !string.IsNullOrWhiteSpace(VirtualMeetingUrl);
        public bool IsPhysical => !string.IsNullOrWhiteSpace(Address) || !string.IsNullOrWhiteSpace(Name);
        public bool HasCoordinates => Latitude.HasValue && Longitude.HasValue;

        public string GetDisplayLocation()
        {
            if (IsVirtual) return "Virtual Meeting";
            if (!string.IsNullOrWhiteSpace(Name)) return Name;
            if (!string.IsNullOrWhiteSpace(Address)) return Address;
            return "No location specified";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name ?? string.Empty;
            yield return Address ?? string.Empty;
            yield return City ?? string.Empty;
            yield return VirtualMeetingUrl ?? string.Empty;
            yield return Latitude ?? 0;
            yield return Longitude ?? 0;
        }
    }
}
