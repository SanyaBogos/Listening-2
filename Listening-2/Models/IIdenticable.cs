using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace listening.Models
{
    public interface IIdenticable<T>
    {
        T Id { get; }
    }
}
