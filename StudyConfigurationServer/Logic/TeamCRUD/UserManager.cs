using System.Collections.Generic;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.TeamCRUD
{
    public class UserManager
    {
        private readonly TeamStorageManager _storageManager;
        public UserManager()
        {
            _storageManager = new TeamStorageManager();
        }

        public UserManager(TeamStorageManager storageManager)
        {
            _storageManager = storageManager;
        }
        public int CreateUser(UserDTO userDto)
        {
            var userToAdd = new User
            {
                Name = userDto.Name,
                Metadata = userDto.Metadata
            };
            
            return _storageManager.SaveUser(userToAdd);
        }

        public bool RemoveUser(int userId)
        {
            return _storageManager.RemoveUser(userId);
        }

        public bool UpdateUser(int userId, UserDTO newUserDto)
        {
            var updatedUser = new User()
            {
                Id = userId,
                Name = newUserDto.Name,
                Metadata = newUserDto.Metadata
            };
            return _storageManager.UpdateUser(updatedUser);
        }

        public IEnumerable<UserDTO> SearchUserDTOs(string userName)
        {
            return
                (from User dbUser in _storageManager.GetAllUsers()
                    where dbUser.Name.Equals(userName)
                    select new UserDTO()
                    {
                        Id = dbUser.Id,
                        Name = dbUser.Name,
                        Metadata = dbUser.Metadata
                    })
                    .ToList();
        }

        public UserDTO GetUserDTO(int userId)
        {
            var dbUser = _storageManager.GetUser(userId);
            return new UserDTO()
            {
                Id = dbUser.Id,
                Name = dbUser.Name,
                Metadata = dbUser.Metadata
            };
        }

        public IEnumerable<UserDTO> GetAllUserDTOs()
        {
            return
                (from User dbUser in _storageManager.GetAllUsers()
                    select new UserDTO()
                    {
                        Id = dbUser.Id,
                        Name = dbUser.Name,
                        Metadata = dbUser.Metadata
                    }).ToList();

        }
    }
} 
