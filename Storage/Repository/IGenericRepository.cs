using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storage.Repository
{
    public interface IGenericRepository : IDisposable
    {
        IQueryable<T> Read<T>() where T : class, IEntity;
        T Read<T>(int id) where T : class, IEntity;
        int Create<T>(T entity) where T : class, IEntity;
        bool Update<T>(T entity) where T : class, IEntity;
        bool Delete<T>(T entity) where T : class, IEntity;
    }
}
