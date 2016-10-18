using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zebpay.Data.Entities.Configuration;
using ZebPay.Core.Entities;

namespace Zebpay.Data.DataContext
{
    class ZebpayDbContext:DbContext
    {
        #region Member Variables

        bool isDisposed;

        #endregion Member Variables

        #region Properties

        public virtual DbSet<RatePublisher> RatePublisher { get; set; }

        #endregion  Properties

        #region Constructores
        public ZebpayDbContext()
        {
            Database.SetInitializer<ZebpayDbContext>(null);
        }
        public ZebpayDbContext(string databaseName):base(databaseName)
        {
            Database.SetInitializer<ZebpayDbContext>(null);
        }
        public ZebpayDbContext(DbConnection connection,bool ownsConnection) : base(connection,contextOwnsConnection:ownsConnection)
        {
            Database.SetInitializer<ZebpayDbContext>(null);
        }
        #endregion Constructores

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new RatePublisherConfiguration());
        }

        protected override void Dispose(bool disposing)
        {
            if(isDisposed)
            {
                if(disposing)
                {

                }
                isDisposed = true;
            }


            base.Dispose(disposing);
        }

        public override async Task<int> SaveChangesAsync()
        {
            return await SaveChangesAsync(CancellationToken.None);
        }
    }
}
