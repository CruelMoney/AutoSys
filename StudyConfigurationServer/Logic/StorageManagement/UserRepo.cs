using System.Data.Entity;
using System.Linq;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public class UserRepo
    {
        private readonly StudyContext _context;

        public UserRepo()
        {
            _context = new StudyContext();
        }

        public UserRepo(StudyContext context)
        {
            _context = context;
        }

        public int CreateUser(User entity)
        {
            _context.Set<User>().Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }

        public bool Delete(User user)
        {
            var found = _context.Set<User>().FindAsync(user.Id);

            if (found == null)
            {
                return false;
            }

            _context.Set<User>().Remove(user);
            _context.SaveChanges();
            return true;
        }

        public IQueryable<User> Read()
        {
            return _context.Set<User>();
        }

        public User Read(int userID)
        {
            return _context.Set<User>().Find(userID);
        }

        public bool UpdateUser(User entity)
        {

            var found = _context.Set<User>().Find(entity.Id);

            if (found == null)
            {
                return false;
            }

            _context.Set<User>().Attach(entity);
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