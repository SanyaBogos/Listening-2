using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace listening.Exceptions
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message) : base(message) { }
    }
}
