using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NtierAppDemo.Data.Entities.Configuration;
using NtierAppDemo.Core.Entities;

namespace NtierAppDemo.Data.DataContext
{
    class NtierAppDemoDbContext:DbContext
    {
        #region Member Variables

        bool isDisposed;

        #endregion Member Variables

        #region Properties

        public virtual DbSet<RatePublisher> RatePublisher { get; set; }

        #endregion  Properties

        #region Constructores
        public NtierAppDemoDbContext()
        {
            Database.SetInitializer<NtierAppDemoDbContext>(null);
        }
        public NtierAppDemoDbContext(string databaseName):base(databaseName)
        {
            Database.SetInitializer<NtierAppDemoDbContext>(null);
        }
        public NtierAppDemoDbContext(DbConnection connection,bool ownsConnection) : base(connection,contextOwnsConnection:ownsConnection)
        {
            Database.SetInitializer<NtierAppDemoDbContext>(null);
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
