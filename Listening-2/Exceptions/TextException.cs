﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace listening.Exceptions
{
    public class TextException : Exception
    {
        public TextException(string message) : base(message) { }
    }
}
