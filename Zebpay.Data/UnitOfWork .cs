using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zebpay.Data.Repositories;
using ZebPay.Core.Entities;
using ZebPay.Core.Repositories.Contracts;

namespace Zebpay.Data
{
    internal class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region Member Variables

        private DbContext dbContext = null;
        private bool disposed = false;

        private IRepository<RatePublisher> ratePublisherRepository = null;
       

        #endregion Member Variables

        #region Properties

        public IRepository<RatePublisher> RatePublisherRepository
        {
            get { return ratePublisherRepository ?? (ratePublisherRepository = new RatePublisherRepository(dbContext)); }
        }
      

        #endregion Properties

        #region Constructors

        public UnitOfWork(DbContext context)
        {
            dbContext = context;
        }

        #endregion Constructors

        #region Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }

            disposed = true;
        }

        public virtual void Commit()
        {
            dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Methods
    }

}
