using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListeningTest
{
    public class BaseTest<T> where T : class, new()
    {
        protected T sut;
        public BaseTest()
        {
            sut = new T();
        }
    }
}
