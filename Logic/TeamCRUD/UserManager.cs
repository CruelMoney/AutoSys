using System.Collections.Generic;
using System.Linq;
using Logic.Model;
using Logic.Model.DTO;
using Logic.StorageManagement;

namespace Logic.TeamCRUD
{
    public class UserManager
    {
        private readonly UserStorageManager _userStorageManager;
        public UserManager()
        {
            _userStorageManager = new UserStorageManager();
        }

        public UserManager(UserStorageManager storageManager)
        {
            _userStorageManager = storageManager;
        }
        public int CreateUser(User user)
        {
            var userToAdd = new UserLogic
            {
                Name = user.Name,
                Metadata = user.Metadata
            };
            
            return _userStorageManager.SaveUser(userToAdd);
        }

        public bool RemoveUser(int userId)
        {
            return _userStorageManager.RemoveUser(userId);
        }

        public bool UpdateUser(int userId, User newUser)
        {
            var updatedUser = new UserLogic()
            {
                Id = userId,
                Name = newUser.Name,
                Metadata = newUser.Metadata
            };
            return _userStorageManager.UpdateUser(updatedUser);
        }

        public IEnumerable<User> SearchUsers(string userName)
        {
            return
                (from UserLogic dbUser in _userStorageManager.GetAllUsers()
                    where dbUser.Name.Equals(userName)
                    select new User()
                    {
                        Id = dbUser.Id,
                        Name = dbUser.Name,
                        Metadata = dbUser.Metadata
                    })
                    .ToList();
        }

        public User GetUser(int userId)
        {
            var dbUser = _userStorageManager.GetUser(userId);
            return new User()
            {
                Id = dbUser.Id,
                Name = dbUser.Name,
                Metadata = dbUser.Metadata
            };
        }
    }
}
