﻿namespace Checkout.Marketplace
{
    public class Capabilities
    {
        public PaymentsNC Payments { get; set; }

        public PayoutsNC Payouts { get; set; }

        public class PaymentsNC
        {
            public bool? Enabled { get; set; }
        }

        public class PayoutsNC
        {
            public bool? Enabled { get; set; }
        }
    }
}