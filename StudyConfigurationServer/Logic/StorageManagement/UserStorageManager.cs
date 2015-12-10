using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Storage.Repository;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public class UserStorageManager
    {
        private readonly IGenericRepository _userRepo;
        public UserStorageManager()
        {
            var context = new StudyContext();
            _userRepo = new EntityFrameworkGenericRepository<StudyContext>(context);
        }

        public UserStorageManager(IGenericRepository repo)
        {
            _userRepo = repo;
        }

        public int SaveUser(User userToSave)
        {
            return _userRepo.Create(userToSave);
        }

        public bool RemoveUser(int userWithIdToDelete)
        {
           return _userRepo.Delete(_userRepo.Read<User>(userWithIdToDelete));
        }

        public bool UpdateUser(User user)
        {
            var userstored = _userRepo.Read<User>(user.Id);
            userstored.Name = user.Name;
            userstored.Metadata = user.Metadata;
           return _userRepo.Update(userstored);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepo.Read<User>();
        }

        public User GetUser(int userId)
        {
           return _userRepo.Read<User>(userId); 
        }

    }
}
