using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDBTest.Interfaces
{
    internal interface IService<T>
    {

        string ConnectionName { set; get; }

        List<T> GetAll();

        void Update(T data);

        void Delete(T data);

        void DeleteAll();

        void Insert(T data);
    }
}
