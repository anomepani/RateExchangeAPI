using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZebPay.Core.Entities.Contracts
{
    public interface IEntity : ISetProperty, IValidatableObject
    {
        #region Properties

        DateTime TimeStamp { get; set; }

       

        #endregion Properties
    }

}
