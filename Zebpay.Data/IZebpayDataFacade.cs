using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZebPay.Core.Repositories.Contracts;

namespace Zebpay.Data
{
    public interface IZebpayDataFacade
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
