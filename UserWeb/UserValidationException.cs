using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserWeb
{
    public class UserValidationException : Exception
    {
        public object DetailMessage { get; set; }

        public UserValidationException(string message) : base(message) { }
    }
}
