using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtierAppDemo.Core.Entities
{
    public class RatePublisher:Entity
    {
        public int Id { get; set; }
        public string FromCurrancy { get; set; }
        public decimal FromAmount { get; set; }
        public string ToCurrancy { get; set; }
        public decimal ToAmount { get; set; }
     

        public RatePublisher()
        {
            RegisterValidationMethods();
            ResetProperties();
        }

        protected override void RegisterValidationMethods()
        {
           
        }

        protected override void ResetProperties()
        {
           
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
        protected virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if(disposing)
                {

                }
            }
            disposed = true;
        }

    }
}
