using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NtierAppDemo.Data;
using NtierAppDemo.Core.Entities;
using NtierAppDemo.Core.Services.Contracts;

namespace NtierAppDemo.Domain.Services
{
    public class RatePublisherService : IRatePublisherService
    {
        public bool Add(RatePublisher rate)
        {
            INtierAppDemoDataFacade facade = new NtierAppDemoDataFacade();
            var unitOfWork = facade.GetUnitOfWork();
            var result = unitOfWork.RatePublisherRepository.Add(rate);
            facade.ReturnUnitOfWork();
            return result;
        }

        public RatePublisher GetRate(string currancyCode)
        {
            INtierAppDemoDataFacade facade = new NtierAppDemoDataFacade();
            var unitOfWork = facade.GetUnitOfWork();
            var rate = unitOfWork.RatePublisherRepository.Find(s => s.FromCurrancy == currancyCode).OrderByDescending(s => s.TimeStamp).FirstOrDefault();
            facade.ReturnUnitOfWork();
            return rate;
        }

    }
}
