#region Using

using System;
using System.Data.Entity;
using System.Linq;

#endregion

namespace Storage.Repository
{
    //Author Jacob Cholewa
    /// <summary>
    /// Implementation of Entity framework
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
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
            return entity.ID;
        }

        public bool Delete<T>(T entity) where T : class, IEntity
        {
            try
            {
                var found = _context.Set<T>().FindAsync(entity.ID);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("Item could not be found in the repository");
            }

            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
            return true;
        }

        public IQueryable<T> Read<T>() where T : class, IEntity
        {
            try
            {
                return _context.Set<T>();
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("Item could not be found in the repository");
            }
        }

        public T Read<T>(int id) where T : class, IEntity
        {
            try
            {
                return _context.Set<T>().Find(id);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("Item could not be found in the repository");
            }
        }

        public bool Update<T>(T entity) where T : class, IEntity
        {
            try
            {
                var found = _context.Set<T>().Find(entity.ID);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("item could not be found in the repository");
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

