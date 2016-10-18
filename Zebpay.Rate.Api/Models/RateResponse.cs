using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZebpayRateExchangeAPI.Models
{
    public class RateResponse: APIResponseModel
    {
        public string SourceCurrency { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
       
    }
}