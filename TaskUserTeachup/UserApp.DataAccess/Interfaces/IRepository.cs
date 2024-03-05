using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApp.DataAccess.Interfaces
{
    public interface IRepository<T>
    {
        T Get(int id);
        void Update(T entity);
        void Delete(T entity);
    }
}
