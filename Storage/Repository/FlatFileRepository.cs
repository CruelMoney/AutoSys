using System;
using System.Collections.Generic;
using System.Linq;

namespace Storage.Repository
{
    public class FlatFileInventory<T> : IRepository where T : IEntity
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

        public void Create<T1>(T1 entity) where T1 : class, IEntity
        {
            throw new NotImplementedException();
        }

        public void Update<T1>(T1 entity) where T1 : class, IEntity
        {
            throw new NotImplementedException();
        }

        public void Delete<T1>(T1 entity) where T1 : class, IEntity
        {
            throw new NotImplementedException();
        }
    }
}
