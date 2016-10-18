using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZebpayRateExchangeAPI.Models
{
    public class APIResponseModel
    {
        public int returncode { get; set; }
        public string err { get; set; }
        public long timestamp { get; set; }
    }
}