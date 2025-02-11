﻿using System.Runtime.Serialization;

namespace Checkout.Common.Four
{
    public enum AccountType
    {
        [EnumMember(Value = "savings")] Savings,
        [EnumMember(Value = "current")] Current,
        [EnumMember(Value = "cash")] Cash
    }
}