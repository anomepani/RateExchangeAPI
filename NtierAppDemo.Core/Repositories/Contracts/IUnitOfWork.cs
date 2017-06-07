using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NtierAppDemo.Core.Entities;

namespace NtierAppDemo.Core.Repositories.Contracts
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
