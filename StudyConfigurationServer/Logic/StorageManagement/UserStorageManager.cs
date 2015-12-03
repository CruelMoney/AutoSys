using System.Collections.Generic;
using System.Data.Entity;
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
            _userRepo = new EntityFrameworkGenericRepository<StudyDataContext>();
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
           return _userRepo.Update(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepo.Read<User>().Include(u=>u.Id);
        }

        public User GetUser(int userId)
        {
           return _userRepo.Read<User>(userId); 
        }

    }
}
