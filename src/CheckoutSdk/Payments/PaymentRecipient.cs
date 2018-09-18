﻿using Newtonsoft.Json;
using System;

namespace Checkout.Payments
{
    public class PaymentRecipient
    {
        [JsonConverter(typeof(DateTimeFormatConverter), "yyyy-MM-dd")]
        public DateTime Dob { get; set; }
        public string AccountNumber { get; set; }
        public string Zip { get; set; }
        public string LastName { get; set; }
    }
}