using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zebpay.Data;
using ZebPay.Core.Entities;
using ZebPay.Core.Services.Contracts;

namespace Zebpay.Domain.Services
{
    public class RatePublisherService : IRatePublisherService
    {
        public bool Add(RatePublisher rate)
        {
            IZebpayDataFacade facade = new ZebpayDataFacade();
            var unitOfWork = facade.GetUnitOfWork();
            var result = unitOfWork.RatePublisherRepository.Add(rate);
            facade.ReturnUnitOfWork();
            return result;
        }

        public RatePublisher GetRate(string currancyCode)
        {
            IZebpayDataFacade facade = new ZebpayDataFacade();
            var unitOfWork = facade.GetUnitOfWork();
            var rate = unitOfWork.RatePublisherRepository.Find(s => s.FromCurrancy == currancyCode).OrderByDescending(s => s.TimeStamp).FirstOrDefault();
            facade.ReturnUnitOfWork();
            return rate;
        }

    }
}
