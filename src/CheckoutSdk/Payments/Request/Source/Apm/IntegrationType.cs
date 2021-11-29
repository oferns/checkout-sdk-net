﻿using System.Runtime.Serialization;

namespace Checkout.Payments.Request.Source.Apm
{
    public enum IntegrationType
    {
        [EnumMember(Value = "direct")] Direct,

        [EnumMember(Value = "redirect")] Redirect,
    }
}