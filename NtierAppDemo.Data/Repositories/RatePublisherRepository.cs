using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NtierAppDemo.Core.Entities;
using NtierAppDemo.Core.Repositories.Contracts;

namespace NtierAppDemo.Data.Repositories
{
    public class RatePublisherRepository : Repository<RatePublisher>, IRatePublisherRepository
    {
        public RatePublisherRepository(DbContext dataContext) : base(dataContext)
        {
        }

    }
}
