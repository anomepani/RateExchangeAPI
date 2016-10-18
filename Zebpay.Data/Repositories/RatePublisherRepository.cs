using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZebPay.Core.Entities;
using ZebPay.Core.Repositories.Contracts;

namespace Zebpay.Data.Repositories
{
    public class RatePublisherRepository : Repository<RatePublisher>, IRatePublisherRepository
    {
        public RatePublisherRepository(DbContext dataContext) : base(dataContext)
        {
        }

    }
}
