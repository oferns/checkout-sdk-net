﻿namespace Checkout.Sessions.Completion
{
    public class NonHostedCompletionInfo : CompletionInfo
    {
        public string CallbackUrl { get; set; }

        public NonHostedCompletionInfo() : base(CompletionInfoType.NonHosted)
        {
        }
    }
}