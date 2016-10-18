using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Zebpay.Bootstrapper;
using ZebPay.Core.Exceptions;
using ZebPay.Core.Services.Contracts;
using ZebpayRateExchangeAPI.Models;

namespace ZebpayRateExchangeAPI.Controllers
{
    public class RateController : ApiController
    {
      
        [System.Web.Mvc.OutputCache(Duration =120,VaryByParam = "currencyCode")]
        [Route("api/v0/rate")]
        [HttpPost]
        public RateResponse GetRate(RequestModel model)
        {
          
            if(model==null)
            {
                return new RateResponse() { err="Invalid request",returncode=400,timestamp=DateTime.UtcNow.Ticks};
            }
            decimal amount = model.amount;
            if(amount == 0)
            {
                amount = 1;
            }
            string code = model.currencyCode.ToUpper();
            List<string> fromCurrancy = new List<string> { "USD", "GBP", "AUD", "EUR", "CAD", "SGD" };
            if (code==string.Empty|| !fromCurrancy.Contains(code))
            {
               code = "USD";
            }
            var ratePublisherService = Bootstrapper.Locate<IRatePublisherService>();
            var rate = ratePublisherService.GetRate(code);
            if(rate==null)
            {
                return new RateResponse() { err = "No data found", returncode = 404, timestamp = DateTime.UtcNow.Ticks };
            }
            var result = new RateResponse();
            result.SourceCurrency = rate.FromCurrancy;
            result.ConversionRate = rate.ToAmount;
            result.Amount = amount;
            result.Total = amount * rate.ToAmount;
            result.returncode = 1;
            result.err = "success";
            result.timestamp = DateTime.UtcNow.Ticks;

            return result;
        }

    }
   
}
