using Checkout.Common;

namespace Checkout.Events
{
    public class EventNotificationSummaryResponse : Resource
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public bool? Success { get; set; }
    }
}