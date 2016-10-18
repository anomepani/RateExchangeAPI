using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZebPay.Core.Entities;

namespace ZebPay.Core.Services.Contracts
{
    public interface IRatePublisherService:IService
    {
        RatePublisher GetRate(string currancyCode);

        bool Add(RatePublisher rate);
    }
}
