#region Using

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

#endregion

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

        public int CreateUser(UserDto userDto)
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
            try
            {
                var user = _storageManager.GetAllUsers()
                    .Where(u => u.ID == userId)
                    .Include(u => u.Teams.Select(t => t.Studies))
                    .FirstOrDefault();

                if (user.Teams.Any(t => t.Studies.Any()))
                {
                    throw new ArgumentException("User is part of one or more studies, and can therefore not be deleted");
                }
                return _storageManager.RemoveUser(userId);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("User could not be found, probably doesn't exist in database");
            }
        }

        public bool UpdateUser(int userId, UserDto newUserDto)
        {
            try
            {
                var userToUpdate = _storageManager.GetUser(userId);

                userToUpdate.Name = newUserDto.Name;
                userToUpdate.Metadata = newUserDto.Metadata;

                return _storageManager.UpdateUser(userToUpdate);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("user could not be found, probably doesn't exist in database");
            }
        }

        public IEnumerable<UserDto> SearchUserDtOs(string userName)
        {
            try
            {
                var users = _storageManager.GetAllUsers();
                return
                    (from User dbUser in _storageManager.GetAllUsers()
                        where dbUser.Name.Equals(userName)
                        select new UserDto
                        {
                            Id = dbUser.ID,
                            Name = dbUser.Name,
                            Metadata = dbUser.Metadata
                        })
                        .ToList();
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("There are no users in the database");
            }
        }

        public UserDto GetUserDto(int userId)
        {
            try
            {
                var dbUser = _storageManager.GetUser(userId);

                return new UserDto
                {
                    Id = dbUser.ID,
                    Name = dbUser.Name,
                    Metadata = dbUser.Metadata
                };
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("User could not be found, probably doesn't exist in database");
            }
        }

        public IEnumerable<UserDto> GetAllUserDtOs()
        {
            try
            {
                var users = _storageManager.GetAllUsers();


                return
                    (from User dbUser in _storageManager.GetAllUsers()
                        select new UserDto
                        {
                            Id = dbUser.ID,
                            Name = dbUser.Name,
                            Metadata = dbUser.Metadata
                        }).ToList();
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("There are no users in the database");
            }
        }

        public IEnumerable<int> GetStudyIds(int userId)
        {
            try
            {
                var user = _storageManager.GetAllUsers()
                    .Where(u => u.ID == userId)
                    .Include(u => u.Teams.Select(t => t.Studies))
                    .FirstOrDefault();

                return user.Teams.SelectMany(t => t.Studies.Select(s => s.ID)).ToList();
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("Could not find the user, probably doesn't exist in the database");
            }
        }
    }
}