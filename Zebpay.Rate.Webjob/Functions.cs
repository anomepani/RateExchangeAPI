using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using RestSharp;
using System.Text.RegularExpressions;
using ZebPay.Core.Entities;
using Zebpay.Bootstrapper;
using ZebPay.Core.Services.Contracts;

namespace Zebpay.Rate.Webjob
{
    public class Functions
    {
        [NoAutomaticTriggerAttribute]
        public static async Task ProcessMethod()
        {

            string toCurrancy = "INR";
            List<string> fromCurrancy = new List<string> { "USD", "GBP", "AUD", "EUR", "CAD", "SGD" };

            while (true)
            {
                try
                {
                    var ratePublisherService = Bootstrapper.Bootstrapper.Locate<IRatePublisherService>();
                    foreach (var currancy in fromCurrancy)
                    {
                        decimal rate = GetRate(currancy);
                        if (rate <= 0)
                        {
                            continue;
                        }

                        RatePublisher newRate = new RatePublisher();
                        newRate.FromCurrancy = currancy;
                        newRate.FromAmount = 1;
                        newRate.ToCurrancy = toCurrancy;
                        newRate.ToAmount = rate;
                        newRate.TimeStamp = DateTime.UtcNow;
                        ratePublisherService.Add(newRate);

                    }
                }
                catch (Exception ex)
                {

                }
                await Task.Delay(TimeSpan.FromMinutes(500));
            }
        }

        public static decimal GetRate(string currancyCode)
        {
            decimal rate = 0;
            try
            {
                RestClient client = new RestClient("https://www.google.com");
                //Code Attribution:
                //Code Reference: http://stackoverflow.com/questions/25302907/google-currency-converter-api-error

                string usd = String.Format("finance/converter?a={0}&from={1}&to={2}&meta={3}", 1, currancyCode, "INR", Guid.NewGuid().ToString());
                RestRequest request = new RestRequest(usd, Method.GET);
                // request.AddParameter("Authorization", string.Format("Bearer " + access_token), ParameterType.HttpHeader);
                var response = client.Execute(request).Content;
                var result = Regex.Matches(response, "<span class=\"?bld\"?>([^<]+)</span>")[0].Groups[1].Value;
                decimal.TryParse(result.Replace("INR", "").Trim(), out rate);
            }
            catch (Exception ex)
            {

            }
            return rate;

        }
    }
}
