using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NtierAppDemo.Core.Entities.Contracts;

namespace NtierAppDemo.Core.Entities.Contracts
{
    public interface IObjectState
    {
        #region Properties

        [NotMapped]
        IObjectState ObjectState { get; set; }

        #endregion Properties
    }
}
