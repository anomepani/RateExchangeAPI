using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtierAppDemo.Core.Exceptions
{
    public class CustomeValidationException : Exception
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public CustomeValidationException(int code,string message)
        {
            Code = code;
            Message = message;
        }

    }
}
