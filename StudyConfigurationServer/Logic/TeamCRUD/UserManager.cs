using System.Collections.Generic;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.TeamCRUD
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
        public int CreateUser(UserDTO userDto)
        {
            var userToAdd = new User
            {
                Name = userDto.Name,
                Metadata = userDto.Metadata
            };
            
            return _userStorageManager.SaveUser(userToAdd);
        }

        public bool RemoveUser(int userId)
        {
            return _userStorageManager.RemoveUser(userId);
        }

        public bool UpdateUser(int userId, UserDTO newUserDto)
        {
            var updatedUser = new User()
            {
                Id = userId,
                Name = newUserDto.Name,
                Metadata = newUserDto.Metadata
            };
            return _userStorageManager.UpdateUser(updatedUser);
        }

        public IEnumerable<UserDTO> SearchUsers(string userName)
        {
            return
                (from User dbUser in _userStorageManager.GetAllUsers()
                    where dbUser.Name.Equals(userName)
                    select new UserDTO()
                    {
                        Id = dbUser.Id,
                        Name = dbUser.Name,
                        Metadata = dbUser.Metadata
                    })
                    .ToList();
        }

        public UserDTO GetUser(int userId)
        {
            var dbUser = _userStorageManager.GetUser(userId);

            return new UserDTO()
            {
                Id = dbUser.Id,
                Name = dbUser.Name,
                Metadata = dbUser.Metadata
            };
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            return
                (from User dbUser in _userStorageManager.GetAllUsers()
                    select new UserDTO()
                    {
                        Id = dbUser.Id,
                        Name = dbUser.Name,
                        Metadata = dbUser.Metadata
                    }).ToList();

        }
    }
}
