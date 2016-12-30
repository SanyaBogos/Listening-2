using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace listening.Exceptions
{
    public class FileUploadException : Exception
    {
        public FileUploadException(string message) : base(message) { }
    }
}
