using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace listening.Repositories
{
    public interface IRepository<T,Y>
    {
        IQueryable<T> GetAll();
        T GetById(Y id);
        void Insert(T item);
        void Update(T item);
        void Delete(Y itemId);
    }
}
