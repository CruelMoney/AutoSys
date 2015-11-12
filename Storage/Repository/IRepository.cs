using System;
using System.Collections.Generic;
using System.Linq;

namespace Storage.Repository
{
    public interface IRepository : IDisposable
    {
        IQueryable<T> Read<T>() where T : class, IEntity;
        T Read<T>(int id) where T : class, IEntity;
        void Create<T>(T entity) where T : class, IEntity;
        void Update<T>(T entity) where T : class, IEntity;
        void Delete<T>(T entity) where T : class, IEntity;
    }
}
