using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZebPay.Core.Entities;

namespace ZebPay.Core.Repositories.Contracts
{
    public interface IUnitOfWork
    {
        #region Properties

        IRepository<RatePublisher> RatePublisherRepository { get; }

        #endregion Properties

        #region Methods

        void Commit();

        #endregion Methods
    }
}
