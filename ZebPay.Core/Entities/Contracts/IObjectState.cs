using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZebPay.Core.Entities.Contracts;

namespace ZebPay.Core.Entities.Contracts
{
    public interface IObjectState
    {
        #region Properties

        [NotMapped]
        IObjectState ObjectState { get; set; }

        #endregion Properties
    }
}
