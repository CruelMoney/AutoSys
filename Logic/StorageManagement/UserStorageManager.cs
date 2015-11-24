using System;
using System.Collections.Generic;
using Logic.Model.DTO;
using Storage.Repository;
using Logic.Model.Data;

namespace Logic.StorageManagement
{
    public class UserStorageManager
    {
        IRepository _userRepo;
        public UserStorageManager()
        {
        }

        public UserStorageManager(IRepository repo)
        {
            _userRepo = repo;
        }

        public void CreateUser(string UserName, string MetaData)
        {
            throw new NotImplementedException();
        }
        public void RemoveUser(int UserID)
        {
            throw new NotImplementedException();
        }
        public void UpdateUser(int UserID, String UpdatedName, String UpdatedMetaData)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<User> SearchUsers(String TeamName)
        {
            throw new NotImplementedException();
        }
        public User GetUser(int UserID)
        {
            throw new NotImplementedException();
        }

    }
}
