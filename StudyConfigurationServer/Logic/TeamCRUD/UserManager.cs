using System.Collections.Generic;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;
using System;

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
            var user = _storageManager.GetUser(userId);
            if (user.StudyIds != null)
            {
                throw new ArgumentException("User is part of one or more studies, and can therefore not be deleted");
            }
            return _storageManager.RemoveUser(userId);             
        }

        public bool UpdateUser(int userId, UserDTO newUserDto)
        {
            var userToUpdate = _storageManager.GetUser(userId);
            if (userToUpdate == null)
            {
                throw new NullReferenceException("user could not be found, problably doesn't exist in database");
            }
            userToUpdate.Name = newUserDto.Name;
            userToUpdate.Metadata = newUserDto.Metadata;
            /*var updatedUser = new User()
            {
                Id = userId,
                Name = newUserDto.Name,
                Metadata = newUserDto.Metadata
            };*/
            return _storageManager.UpdateUser(userToUpdate);
        }

        public IEnumerable<UserDTO> SearchUsers(string userName)
        {
            var users = _storageManager.GetAllUsers();
            if (users == null)
            {
                throw new NullReferenceException("There are no users in the database");
            }
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

        public UserDTO GetUser(int userId)
        {
            var dbUser = _storageManager.GetUser(userId);
            if(dbUser == null)
            {
                throw new NullReferenceException("User could not be found, probably doesn't exist in database");
            }
            return new UserDTO()
            {
                Id = dbUser.Id,
                Name = dbUser.Name,
                Metadata = dbUser.Metadata
            };
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            var users = _storageManager.GetAllUsers();
            if(users == null)
            {
                throw new NullReferenceException("There are no users in the database");
            }
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
