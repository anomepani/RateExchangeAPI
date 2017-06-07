using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NtierAppDemo.Core.Entities;

namespace NtierAppDemo.Data.Entities.Configuration
{
    class RatePublisherConfiguration:EntityTypeConfiguration<RatePublisher>
    {
        #region Constructor

        public RatePublisherConfiguration()
        {
            HasKey(a => a.Id);
            Property(a => a.FromCurrancy).IsRequired().HasMaxLength(10);
            Property(a => a.FromAmount).IsRequired();
            Property(a => a.ToCurrancy).IsRequired().HasMaxLength(10);
            Property(a => a.ToAmount).IsRequired();
            Property(a => a.TimeStamp).IsOptional();
        }


        #endregion Constructor
    }
}
