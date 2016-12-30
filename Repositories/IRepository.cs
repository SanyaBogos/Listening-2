using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace listening.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        T GetById(string id);
        void Insert(T item);
        void Update(T intem);
        void Delete(string itemId);
    }
}
