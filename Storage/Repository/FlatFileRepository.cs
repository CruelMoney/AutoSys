using System;
using System.Collections.Generic;

namespace Storage.Repository
{
    public class FlatFileInventory<T> : IRepository<T> where T : IEntity
    {

        public FlatFileInventory(string filename)
        {
            throw new NotImplementedException();
        }

        public int Create(T item)
        {
            throw new NotImplementedException();
        }

        public T Read(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Read()
        {
            throw new NotImplementedException();
        }

        public void Update(T item)
        {
            throw new NotImplementedException();
        }

        public void Delete(T item)
        {
            throw new NotImplementedException();
        }

        private bool Save()
        {
            throw new NotImplementedException();
        }

        private bool Load()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}
