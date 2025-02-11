﻿using Checkout.Common;
using System.Collections.Generic;

namespace Checkout.Reconciliation
{
    public class ReconciliationPaymentReportResponse : Resource
    {
        public int? Count { get; set; }

        public IList<PaymentReportData> Data { get; set; }
    }
}