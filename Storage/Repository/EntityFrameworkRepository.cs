using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Storage.Repository
{
    public class EntityFrameworkRepository<TContext> : IRepository where TContext : IDbContext, new()
    {
        private readonly TContext _context;

        public EntityFrameworkRepository()
        {
            _context = new TContext();
        }

        public EntityFrameworkRepository(TContext context)
        {
            _context = context;
        }

        public void Create<T>(T entity) where T : class, IEntity
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Delete<T>(T entity) where T : class, IEntity
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public IQueryable<T> Read<T>() where T : class, IEntity
        {
            return _context.Set<T>();
        }

        public T Read<T>(int id) where T : class, IEntity
        {
            return _context.Set<T>().Find(id);
        }

        public void Update<T>(T entity) where T : class, IEntity
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}

