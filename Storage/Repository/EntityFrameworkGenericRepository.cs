using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Storage.Repository
{
    public class EntityFrameworkGenericRepository<TContext> : IGenericRepository where TContext : IDbContext, new()
    {
        private readonly TContext _context;

        public EntityFrameworkGenericRepository()
        {
            _context = new TContext();
        }

        public EntityFrameworkGenericRepository(TContext context)
        {
            _context = context;
        }

        public int Create<T>(T entity) where T : class, IEntity
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }

        public bool Delete<T>(T entity) where T : class, IEntity
        {
            var found = _context.Set<T>().FindAsync(entity.Id);

            if (found == null)
            {
                return false;
            }

            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
            return true;
        }

        public IQueryable<T> Read<T>() where T : class, IEntity
        {
            return _context.Set<T>();
        }

        public T Read<T>(int id) where T : class, IEntity
        {
            return _context.Set<T>().Find(id);
        }

        public bool Update<T>(T entity) where T : class, IEntity
        {

            var found = _context.Set<T>().Find(entity.Id);

            if (found == null)
            {
                return false;
            }

     
            
            _context.Set<T>().Attach(entity);        
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();

            return true;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}

