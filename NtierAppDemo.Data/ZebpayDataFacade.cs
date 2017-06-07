using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NtierAppDemo.Data.DataContext;
using NtierAppDemo.Core.Repositories.Contracts;

namespace NtierAppDemo.Data
{
    public class NtierAppDemoDataFacade : INtierAppDemoDataFacade
    {
        #region Member Variables

        private DbConnection dbConnection = null;
        private NtierAppDemoDbContext context = null;
        private UnitOfWork unitOfWork;
        private bool firstRequest = true;

        #endregion Member Variables

        #region Properties

        public string ConnectionString { get; set; }

        public bool KeepConnectionAlive { get; set; }

        #endregion Properties

        #region Constructors

        public NtierAppDemoDataFacade()
        {
            ReadAppConfigSettings();
        }

        #endregion Constructors

        #region Methods

        private void ReadAppConfigSettings()
        {
           
            ConnectionString = NtierAppDemo.Core.Utilities.ConfigurationSettings.NtierAppDemoConnectionString;
        }

        public virtual IUnitOfWork GetUnitOfWork()
        {
            if (unitOfWork != null)
            {
                throw new Exception("a unit of work is already in use.");
            }

            else
            {
                if (!KeepConnectionAlive)
                {
                    context = new NtierAppDemoDbContext(ConnectionString);

                    if (firstRequest)
                    {
                        context.Database.Initialize(force: false);
                        firstRequest = false;
                    }
                }
                else
                {
                    if (dbConnection == null)
                    {
                        if (firstRequest)
                        {
                            using (var tempContext = new NtierAppDemoDbContext(ConnectionString))
                            {
                                tempContext.Database.Initialize(force: false);
                                dbConnection = tempContext.Database.Connection;
                            }

                            firstRequest = false;
                        }



                        if (dbConnection != null)
                        {
                            dbConnection.ConnectionString = ConnectionString;
                            dbConnection.Open();
                        }
                    }

                    context = new NtierAppDemoDbContext(dbConnection, false);
                }

                unitOfWork = new UnitOfWork(context);
                return unitOfWork;
            }
        }

        public virtual void ReturnUnitOfWork()
        {
            if (unitOfWork != null)
            {
                if (!KeepConnectionAlive)
                {
                    if (dbConnection != null)
                    {
                        throw new Exception("Database connection is not null in disconnected state.");
                    }
                }

                else if (dbConnection == null || dbConnection.State != System.Data.ConnectionState.Open)
                {
                    throw new Exception("Database connection is not open in connected state.");
                }

                unitOfWork.Dispose();
                unitOfWork = null;
                context = null;
            }
        }

        #endregion Methods
    }
}
