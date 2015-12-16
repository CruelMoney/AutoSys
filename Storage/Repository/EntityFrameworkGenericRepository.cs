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

        /// <summary>
        /// Add an entity to the repository and return the Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The object to store</param>
        /// <returns></returns>
        public int Create<T>(T entity) where T : class, IEntity
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return entity.ID;
        }

        /// <summary>
        /// Delete an entity from the repository
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The object to be deleted</param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieve all isntances of a given object in the repository
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieve a single object with a given Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">Id of the object to be returned</param>
        /// <returns></returns>
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

        /// <summary>
        /// Update an object in the repository
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">Object to be updated</param>
        /// <returns></returns>
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

