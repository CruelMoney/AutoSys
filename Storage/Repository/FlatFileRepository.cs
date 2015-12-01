using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storage.Repository
{
    public class FlatFileInventory<T> : IGenericRepository where T : IEntity
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T1> Read<T1>() where T1 : class, IEntity
        {
            throw new NotImplementedException();
        }

        public T1 Read<T1>(int id) where T1 : class, IEntity
        {
            throw new NotImplementedException();
        }

        public int Create<T1>(T1 entity) where T1 : class, IEntity
        {
            throw new NotImplementedException();
        }

        public bool Update<T1>(T1 entity) where T1 : class, IEntity
        {
            throw new NotImplementedException();
        }

        public bool Delete<T1>(T1 entity) where T1 : class, IEntity
        {
            throw new NotImplementedException();
        }
    }
}
