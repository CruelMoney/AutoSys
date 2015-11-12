using System.Collections.Generic;

namespace Storage.Repository
{
    public interface IRepository<T> where T : IEntity
    {
        int Create(T item);
        T Read(int id);
        IEnumerable<T> Read();
        void Update(T item);
        void Delete(T item);
        void Dispose();
    }
}
