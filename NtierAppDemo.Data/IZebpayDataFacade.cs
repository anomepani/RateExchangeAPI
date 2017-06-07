using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NtierAppDemo.Core.Repositories.Contracts;

namespace NtierAppDemo.Data
{
    public interface INtierAppDemoDataFacade
    {
        #region Properties

        bool KeepConnectionAlive { get; set; }

        string ConnectionString { get; set; }

        #endregion Properties

        #region Methods

        IUnitOfWork GetUnitOfWork();

        void ReturnUnitOfWork();

        #endregion Methods        
    }
}
