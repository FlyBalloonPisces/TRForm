using LiteDBTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDBTest.Services
{
    internal class Service<T> : IService<T>
    {
        public T value { get; set; }

        public string ConnectionName { get; set; }

        public Service(T _value)
        {
            this.value = _value;
            this.ConnectionName = _value.GetType().Name;
        }

        public void Delete(T data)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Insert(T data)
        {
            throw new NotImplementedException();
        }

        public void Update(T data)
        {
            throw new NotImplementedException();
        }
    }
}
